using DocumentFormat.OpenXml.Office2021.PowerPoint.Comment;
using QrAssignment.Application.Common.DTOs;
using System.Net.NetworkInformation;

namespace QrAssignment.Application.Features.Feedbacks.DTOs
{
    public class FeedBackListItemDto : BaseListItemDto
    {
        public FeedBackListItemDto() { }

        public FeedBackListItemDto(Guid? id, long revNum, string modifiedUserFullName, string createdUserFullName,
            DateTimeOffset? modifiedDateTime, DateTimeOffset createdDateTime, string comment, string pageUrl, int status, string screenshotPath)
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
            ScreenshotPath = screenshotPath;

        }

        public Guid? Id { get; set; }
        public string Comment { get; set; }

        public string PageUrl { get; set; }
        public string CreatorEmail { get; set; }

        public int Status { get; set; }

        public DateTimeOffset CreatedDate { get; set; }


        public string ScreenshotPath { get; set; }
    }
}
