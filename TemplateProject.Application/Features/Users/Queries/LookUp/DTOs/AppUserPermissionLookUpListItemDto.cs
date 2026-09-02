namespace QrAssignment.Application.Features.Users.Queries.LookUp.DTOs
{
    public class PermissionLookUpListItemDto : RoleLookUpListItemDto
    { 
        public bool HasPermission { get; set; }
    }
    public class RoleLookUpListItemDto
    {
        public RoleLookUpListItemDto()
        {

        }
        public RoleLookUpListItemDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; 
    }
}
