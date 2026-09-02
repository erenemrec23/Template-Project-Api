using System.Security.Claims;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Exceptions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Presentation.Middlewares;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IAppLocalizer _localizer;
    private readonly IDbExceptionTranslator _dbExceptionTranslator;
    private readonly IErrorNotifier _errorNotifier;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        IAppLocalizer localizer,
        IDbExceptionTranslator dbExceptionTranslator,
        IErrorNotifier errorNotifier)
    {
        _logger = logger;
        _localizer = localizer;
        _dbExceptionTranslator = dbExceptionTranslator;
        _errorNotifier = errorNotifier;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var path = httpContext.Request.Path;

        // Client bağlantıyı kestiyse bu bir sunucu hatası değil; log/mail üretme.
        if (exception is OperationCanceledException && httpContext.RequestAborted.IsCancellationRequested)
        {
            httpContext.Response.StatusCode = 499;
            return true;
        }

        if (_dbExceptionTranslator.TryTranslate(exception, out var translated))
        {
            _logger.LogWarning(exception, "Veritabanı kısıt ihlali: {RequestPath}", path);
            exception = translated;
        }

        switch (exception)
        {
            case ValidationAppException validationException:
                {
                    var message = validationException.Errors is null || !validationException.Errors.Any()
                        ? _localizer["Validations.ValidationErrors"]
                        : string.Join(". ", validationException.Errors.Select(e => string.Join(". ", e.Value)));

                    await WriteErrorAsync(httpContext, StatusCodes.Status400BadRequest,
                        new
                        {
                            isSuccess = false,
                            isFailure = true,
                            error = new { code = "Validation.Error", message },
                            validationErrors = validationException.Errors
                        }, cancellationToken);
                    return true;
                }

            // DuplicateEntityException : BusinessException → daha spesifik tip önce gelmeli
            case DuplicateEntityException duplicate:
                _logger.LogWarning("Duplicate kayıt: {Message} ({RequestPath})", duplicate.Message, path);
                await WriteErrorAsync(httpContext, StatusCodes.Status409Conflict,
                    Failure("Database.DuplicateKey", duplicate.Message), cancellationToken);
                return true;

            case BusinessException business:
                _logger.LogWarning("İş kuralı ihlali: {Message} ({RequestPath})", business.Message, path);
                await WriteErrorAsync(httpContext, StatusCodes.Status400BadRequest,
                    Failure("BusinessRule.Violation", business.Message), cancellationToken);
                return true;

            case UnauthorizedAccessException unauthorized:
                _logger.LogWarning("Yetkisiz erişim: {RequestPath}", path);
                await WriteErrorAsync(httpContext, StatusCodes.Status403Forbidden,
                    Failure("Authorization.Forbidden", unauthorized.Message), cancellationToken);
                return true;
        }

        // ---- Beklenmeyen hata: logla, mail kuyruğuna at, 500 dön ----
        _logger.LogError(exception, "Kritik Hata: {RequestPath}", path);
        Notify(httpContext, exception);

        var result = Result.Failure(new Error("Server.InternalError", _localizer["Errors.UnKnownException"]));
        await WriteErrorAsync(httpContext, StatusCodes.Status500InternalServerError, result, cancellationToken);
        return true;
    }

    private void Notify(HttpContext ctx, Exception ex)
    {
        try
        {
            var notification = new ErrorNotification(
                OccurredAt: DateTimeOffset.UtcNow,
                TraceId: ctx.TraceIdentifier,
                Method: ctx.Request.Method,
                Path: ctx.Request.Path,
                QueryString: ctx.Request.QueryString.HasValue ? ctx.Request.QueryString.Value : null,
                UserName: ctx.User?.Identity?.IsAuthenticated == true
                    ? ctx.User.FindFirstValue(ClaimTypes.Name) ?? ctx.User.FindFirstValue(ClaimTypes.NameIdentifier)
                    : null,
                ClientIp: ctx.Connection.RemoteIpAddress?.ToString(),
                ExceptionType: ex.GetType().FullName ?? ex.GetType().Name,
                Message: ex.Message,
                Details: ex.ToString()); // inner exception ve stack trace dahil

            if (!_errorNotifier.TryEnqueue(notification))
                _logger.LogDebug("Hata bildirimi kuyruğa alınmadı (kapalı veya kuyruk dolu). TraceId: {TraceId}", ctx.TraceIdentifier);
        }
        catch (Exception notifyEx)
        {
            // Bildirim altyapısı asla asıl hata yanıtını bozmasın.
            _logger.LogWarning(notifyEx, "Hata bildirimi oluşturulamadı.");
        }
    }

    private static object Failure(string code, string message) =>
        new { isSuccess = false, isFailure = true, error = new { code, message } };

    private static Task WriteErrorAsync(HttpContext ctx, int statusCode, object payload, CancellationToken ct)
    {
        ctx.Response.StatusCode = statusCode;
        ctx.Response.ContentType = "application/json";
        return ctx.Response.WriteAsJsonAsync(payload, ct);
    }
}