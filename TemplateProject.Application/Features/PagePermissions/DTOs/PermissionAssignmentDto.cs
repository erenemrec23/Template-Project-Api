namespace QrAssignment.Application.Features.PagePermissions.DTOs
{
    // Update isteği - sadece ID'ler yeterli
    public sealed record PermissionAssignmentDto(string? PageName, string? GroupKey, int PermissionValue);


    public sealed record PermissionAssignmentDto2(
        Guid? UserId,
        Guid? RoleId,
        int PermissionValue);
}
