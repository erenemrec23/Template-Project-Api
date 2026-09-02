namespace QrAssignment.Application.Features.PagePermissions.DTOs
{
    public sealed record PagePermissionAssignmentDto(
       Guid? UserId,
       string? UserName,
       Guid? RoleId,
       string? RoleName,
       int PermissionValue);
}
