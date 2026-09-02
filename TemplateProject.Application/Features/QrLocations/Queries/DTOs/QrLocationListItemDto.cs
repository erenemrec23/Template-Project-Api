using QrAssignment.Application.Common.DTOs;

namespace QrAssignment.Application.Features.QrLocations.Queries.DTOs
{
    public class QrLocationListItemDto : BaseListItemDto
    {
        public QrLocationListItemDto() { }

        public QrLocationListItemDto(Guid? id, string name, DateTimeOffset? startDate, DateTimeOffset? endDate,
            string? locationName, long revNum, string modifiedUserFullName, string createdUserFullName,
            DateTimeOffset modifiedDateTime, DateTimeOffset createdDateTime)
        {
            Id = id;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            LocationName = locationName;
            RevNum = revNum;
            ModifiedUserFullName = modifiedUserFullName;
            CreatedUserFullName = createdUserFullName;
            ModifiedDateTime = modifiedDateTime;
            CreatedDateTime = createdDateTime;
        }

        public Guid? Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public string? LocationName { get; set; }
    }
}
