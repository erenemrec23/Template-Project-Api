namespace QrAssignment.Application.Features.Permission.Commands.Update
{
    public sealed record PermissionUserUpdateDto
    {
        public PermissionUserUpdateDto() { }
        public PermissionUserUpdateDto(string? pageName, string? groupKey, int permissionValue)
        {
            PageName = pageName;
            GroupKey = groupKey;
            PermissionValue = permissionValue;
        }
        public string? PageName { get; init; }   // sayfa hedefli (Page.PageKey)
        public string? GroupKey { get; init; }   // grup hedefli (MenuGroup.Key)
        public int PermissionValue { get; init; }
    }
}
