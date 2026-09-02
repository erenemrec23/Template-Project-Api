
using QrAssignment.Application.Common.DTOs;

namespace QrAssignment.Application.Features.Roles.DTOs
{
    public class RoleItemDto : BaseItemDto
    {
        public RoleItemDto() { }
        public RoleItemDto(Guid? id, string name, byte[] rowVersion)
        {
            Id = id;
            Name = name;
            RowVersion = rowVersion;
        }
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }
}