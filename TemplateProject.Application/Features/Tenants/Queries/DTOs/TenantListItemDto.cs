using QrAssignment.Application.Common.DTOs;

namespace QrAssignment.Application.Features.Tenants.Queries.DTOs
{
    public class TenantListItemDto : BaseListItemDto
    {
        public TenantListItemDto(Guid? id, string name, long revNum, string modifiedUserFullName,
            string createdUserFullName, DateTimeOffset modifiedDateTime, DateTimeOffset createdDateTime) { 
        
            Id = id;
            Name = name;
            RevNum = revNum;
            ModifiedUserFullName = modifiedUserFullName;
            CreatedUserFullName = createdUserFullName;
            ModifiedDateTime = modifiedDateTime;
            CreatedDateTime = createdDateTime;

        }
        public TenantListItemDto()
        { }
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }
    
}
 