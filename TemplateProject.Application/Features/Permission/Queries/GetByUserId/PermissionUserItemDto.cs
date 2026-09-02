namespace QrAssignment.Application.Features.Permission.Queries.GetByUserId
{
    public class PermissionUserPageItemDto
    {
        public string? PageName { get; init; }
        public string? GroupKey { get; init; }
        public int PermissionValue { get; init; }
    }

    public class PermissionUserItemDto : PermissionBaseItemDto
    {
        public Guid? UserId { get; set; } 
    }


    public class PermissionBaseItemDto
    { 
        public List<PermissionUserPageItemDto> PagePermissionList { get; set; } = new List<PermissionUserPageItemDto>();
    }

}
