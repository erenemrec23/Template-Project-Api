namespace QrAssignment.Application.Features.Roles.Commands.DTOs
{
    public sealed record RolePagePermissionDto2(string? PageName, string? GroupKey, int PermissionValue);


    public sealed record RolePagePermissionDto(string? PageName, string? GroupKey, int PermissionValue);
}