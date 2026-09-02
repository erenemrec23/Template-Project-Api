// QrAssignment.Domain/Entity/App/Feedback.cs
using QrAssignment.Domain.Abstractions;

namespace QrAssignment.Domain.Entity.App
{
    public enum FeedbackStatus
    {
        Pending = 1,    // Beklemede
        InProgress = 2, // İncelemede
        Completed = 3   // Tamamlandı
    }

    public class Feedback : BaseEntity
    {
        public string Comment { get; set; } = string.Empty;
        public string? ScreenshotPath { get; set; } // Dosya yolu veya Base64
        public string PageUrl { get; set; } = string.Empty;
        public FeedbackStatus Status { get; set; } = FeedbackStatus.Pending;
         
    }
}