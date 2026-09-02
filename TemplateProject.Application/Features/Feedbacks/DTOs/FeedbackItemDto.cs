namespace QrAssignment.Application.Features.Feedbacks.DTOs
{
    public class FeedbackItemDto : FeedBackListItemDto
    {
        public FeedbackItemDto() { }

        public FeedbackItemDto(Guid? id, long revNum, string modifiedUserFullName, string createdUserFullName,
            DateTimeOffset? modifiedDateTime, DateTimeOffset createdDateTime, string comment, string pageUrl, int status, byte[] rowVersion, string screenshotPath)
        {
            Id = id;
            RevNum = revNum;
            ModifiedUserFullName = modifiedUserFullName;
            CreatedUserFullName = createdUserFullName;
            ModifiedDateTime = modifiedDateTime;
            CreatedDateTime = createdDateTime;
            Comment = comment;
            PageUrl = pageUrl;
            CreatorEmail = createdUserFullName;
            Status = status;
            CreatedDate = createdDateTime;
            RowVersion = rowVersion;
            ScreenshotPath = screenshotPath;
            
        }
         
        public byte[] RowVersion { get; set; }

    }

    
}
