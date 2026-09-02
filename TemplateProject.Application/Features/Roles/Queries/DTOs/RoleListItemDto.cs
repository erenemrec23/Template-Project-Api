using QrAssignment.Application.Common.DTOs;

namespace QrAssignment.Application.Features.Roles.Queries.GetList
{
    // DTO (Client'a tüm AppRole nesnesini değil, sadece gerekenleri dönmek için)
    public class RoleListItemDto : BaseListItemDto 
    {
        public RoleListItemDto(Guid id, string name, long revNum, string modifiedUserFullName,
            string createdUserFullName, DateTimeOffset? modifiedDateTime, DateTimeOffset createdDateTime)
        {

            Id = id;
            Name = name;
            RevNum = revNum;
            ModifiedUserFullName = modifiedUserFullName;
            CreatedUserFullName = createdUserFullName;
            ModifiedDateTime = modifiedDateTime;
            CreatedDateTime = createdDateTime;
        }
        public Guid Id { get; set; }
        public string  Name { get; set; }

    }
}