using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QrAssignment.Application.Common;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;
using System.Net;
using System.Net.Mail;

namespace QrAssignment.Infrastructure.Services
{
    public sealed class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<EmailService> _logger;
        private readonly UserManager<AppUser> _userManager;

        public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger,
            UserManager<AppUser> userManager)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
        {
            // SMTP konfigürasyonu dolu mu kontrolü
            bool useSmtp = !string.IsNullOrWhiteSpace(_mailSettings.Host) &&
                           !string.IsNullOrWhiteSpace(_mailSettings.UserName) &&
                           !string.IsNullOrWhiteSpace(_mailSettings.Password);

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(_mailSettings.FromAddress ?? "noreply@qrassignment.local"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            mailMessage.To.Add(to);

            if (useSmtp)
            {
                // 1. Gerçek SMTP Sunucusu Üzerinden Gönderim
                using var client = new SmtpClient(_mailSettings.Host, _mailSettings.Port)
                {
                    Credentials = new NetworkCredential(_mailSettings.UserName, _mailSettings.Password),
                    EnableSsl = true
                };

                await client.SendMailAsync(mailMessage, cancellationToken);
                _logger.LogInformation("Email başarıyla {To} adresine SMTP üzerinden gönderildi.", to);
            }
            else
            {
                // 2. Local Pickup Directory (Diske .eml Olarak Kaydetme)
                string pickupDirectory = _mailSettings.LocalPickupDirectory ?? Path.Combine(Directory.GetCurrentDirectory(), "SentMails");

                // Klasör mevcut değilse otomatik oluştur
                if (!Directory.Exists(pickupDirectory))
                {
                    Directory.CreateDirectory(pickupDirectory);
                }

                using var client = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                    PickupDirectoryLocation = pickupDirectory
                };

                await client.SendMailAsync(mailMessage, cancellationToken);
                _logger.LogWarning("SMTP ayarları bulunamadı. Email yerel klasöre kaydedildi: {Directory}", pickupDirectory);
            }
        }

        public async Task<Result> ForgotPasswordAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(email);

            // Kullanici enumeration'ini engellemek icin: kullanici bulunamasa bile basarili donuyoruz.
            if (user is null)
                return Result.Success();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Token URL query'sinde tasinacagi icin encode ediyoruz (+ / = karakterleri bozulmasin).
            var resetLink =
                $"{_mailSettings.ClientUrl}/reset-password" +
                $"?email={Uri.EscapeDataString(email)}" +
                $"&token={Uri.EscapeDataString(token)}";

            const string subject = "Şifre Sıfırlama Talebi";
            var body = $@"
        <p>Merhaba,</p>
        <p>Hesabınız için şifre sıfırlama talebinde bulunuldu. Aşağıdaki bağlantıya tıklayarak yeni şifrenizi belirleyebilirsiniz:</p>
        <p><a href=""{resetLink}"">Şifremi Sıfırla</a></p>
        <p>Bu talebi siz yapmadıysanız bu e-postayı yok sayabilirsiniz.</p>";

            await SendEmailAsync(email, subject, body, cancellationToken);

            return Result.Success();
        }
    }
}