using QrAssignment.Application.Features.QrLocations.Queries.DTOs;

namespace QrAssignment.Application.Features.QrLocations.DTOs
{
    public class QrLocationItemDto : QrLocationListItemDto
    {
        public QrLocationItemDto() { }

        public QrLocationItemDto(Guid? id, string name, DateTimeOffset? startDate, DateTimeOffset? endDate,
            string? locationName, long revNum, string modifiedUserFullName, string createdUserFullName,
            DateTimeOffset createdDateTime, DateTimeOffset modifiedDateTime, byte[] rowVersion)
        {
            Id = id;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            LocationName = locationName;
            RevNum = revNum;
            CreatedUserFullName = createdUserFullName;
            ModifiedUserFullName = modifiedUserFullName;
            CreatedDateTime = createdDateTime;
            ModifiedDateTime = modifiedDateTime;
            RowVersion = rowVersion;
        }

        public byte[] RowVersion { get; set; }
    }

    
}
