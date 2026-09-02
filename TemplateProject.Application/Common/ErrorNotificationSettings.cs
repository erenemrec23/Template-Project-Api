namespace QrAssignment.Application.Common;

public sealed class ErrorNotificationSettings
{
    public const string SectionName = "ErrorNotification";

    public bool Enabled { get; set; }
    public string[] Recipients { get; set; } = [];
    /// <summary>Aynı hata (tip+mesaj+path) bu pencere içinde tekrar mail üretmez.</summary>
    public int ThrottleWindowMinutes { get; set; } = 10;
    /// <summary>Pencere başına gönderilecek maksimum farklı hata maili.</summary>
    public int MaxNotificationsPerWindow { get; set; } = 20;
    public int QueueCapacity { get; set; } = 500;
}