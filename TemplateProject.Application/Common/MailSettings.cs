namespace QrAssignment.Application.Common
{
    public sealed class MailSettings
    {
        public string? Host { get; set; }
        public int Port { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? FromAddress { get; set; }

        // SMTP yoksa veya geliştirme aşamasındaysak burası devreye girecek
        public string? LocalPickupDirectory { get; set; }

        public string ClientUrl { get; set; } = string.Empty;
    }
}