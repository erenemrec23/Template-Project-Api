
using Microsoft.VisualBasic;
using QrAssignment.Application.Features.Tenants.Queries.DTOs;

namespace QrAssignment.Application.Features.Tenants.DTOs
{
    public class TenantItemDto : TenantListItemDto
    {
        public TenantItemDto(){ }


        public TenantItemDto(Guid? id, string name, long revNum, string modifiedUserFullName, string createdUserFullName,DateTimeOffset createdDateTime, DateTimeOffset modifiedDateTime, byte[] rowVersion)
        {

            Id = id;
            Name = name;
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
 