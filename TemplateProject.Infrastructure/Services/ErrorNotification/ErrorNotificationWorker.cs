using System.Net;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QrAssignment.Application.Common;
using QrAssignment.Application.Interfaces;

namespace QrAssignment.Infrastructure.Services.ErrorNotification;

public sealed class ErrorNotificationWorker : BackgroundService
{
    private readonly ErrorNotificationChannel _channel;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHostEnvironment _environment;
    private readonly ILogger<ErrorNotificationWorker> _logger;
    private readonly ErrorNotificationSettings _settings;

    // fingerprint -> son gönderim zamanı
    private readonly Dictionary<string, DateTimeOffset> _lastSent = new();
    private DateTimeOffset _windowStart = DateTimeOffset.UtcNow;
    private int _sentInWindow;

    public ErrorNotificationWorker(
        ErrorNotificationChannel channel,
        IServiceScopeFactory scopeFactory,
        IHostEnvironment environment,
        IOptions<ErrorNotificationSettings> options,
        ILogger<ErrorNotificationWorker> logger)
    {
        _channel = channel;
        _scopeFactory = scopeFactory;
        _environment = environment;
        _logger = logger;
        _settings = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var notification in _channel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                if (!ShouldSend(notification))
                    continue;

                await SendAsync(notification, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                // Mail gönderimi patlarsa worker ölmesin; sadece logla.
                _logger.LogError(ex, "Hata bildirim maili gönderilemedi. TraceId: {TraceId}", notification.TraceId);
            }
        }
    }

    private bool ShouldSend(Application.Interfaces.ErrorNotification n)
    {
        var now = DateTimeOffset.UtcNow;
        var window = TimeSpan.FromMinutes(Math.Max(1, _settings.ThrottleWindowMinutes));

        if (now - _windowStart > window)
        {
            _windowStart = now;
            _sentInWindow = 0;
            _lastSent.Clear();
        }

        if (_sentInWindow >= _settings.MaxNotificationsPerWindow)
        {
            _logger.LogWarning("Hata bildirim limiti doldu ({Max}/{Window}dk). TraceId: {TraceId} atlandı.",
                _settings.MaxNotificationsPerWindow, _settings.ThrottleWindowMinutes, n.TraceId);
            return false;
        }

        var fingerprint = $"{n.ExceptionType}|{n.Message}|{n.Method} {n.Path}";
        if (_lastSent.TryGetValue(fingerprint, out var last) && now - last < window)
            return false;

        _lastSent[fingerprint] = now;
        _sentInWindow++;
        return true;
    }

    private async Task SendAsync(Application.Interfaces.ErrorNotification n, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

        var subject = $"[{_environment.ApplicationName}/{_environment.EnvironmentName}] {n.ExceptionType}: {Truncate(n.Message, 80)}";
        var body = BuildBody(n);

        foreach (var recipient in _settings.Recipients)
            await emailService.SendEmailAsync(recipient, subject, body, ct);

        _logger.LogInformation("Hata bildirim maili gönderildi. TraceId: {TraceId}", n.TraceId);
    }

    private string BuildBody(Application.Interfaces.ErrorNotification n)
    {
        static string E(string? s) => WebUtility.HtmlEncode(s ?? "-");

        var sb = new StringBuilder();
        sb.Append("<h3>Beklenmeyen Hata</h3>");
        sb.Append("<table border='0' cellpadding='4' style='font-family:monospace'>");
        Row(sb, "Uygulama", $"{_environment.ApplicationName} ({_environment.EnvironmentName})");
        Row(sb, "Zaman (UTC)", n.OccurredAt.ToString("yyyy-MM-dd HH:mm:ss"));
        Row(sb, "TraceId", n.TraceId);
        Row(sb, "İstek", $"{n.Method} {n.Path}{n.QueryString}");
        Row(sb, "Kullanıcı", n.UserName);
        Row(sb, "IP", n.ClientIp);
        Row(sb, "Hata Tipi", n.ExceptionType);
        Row(sb, "Mesaj", n.Message);
        sb.Append("</table>");
        sb.Append("<h4>Detay</h4>");
        sb.Append("<pre style='font-size:12px;white-space:pre-wrap'>").Append(E(n.Details)).Append("</pre>");
        return sb.ToString();

        static void Row(StringBuilder b, string key, string? value) =>
            b.Append("<tr><td><b>").Append(E(key)).Append("</b></td><td>").Append(E(value)).Append("</td></tr>");
    }

    private static string Truncate(string s, int max) =>
        s.Length <= max ? s : s[..max] + "…";
}