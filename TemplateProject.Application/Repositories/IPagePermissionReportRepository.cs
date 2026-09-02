using QrAssignment.Application.Features.Permission.Queries.DTOs;

namespace QrAssignment.Application.Repositories;

public interface IPagePermissionReportRepository
{
    Task<List<PermissionPageRow>> GetPagesAsync(CancellationToken ct);
    Task<List<PermissionSourceRow>> GetSourceRowsAsync(CancellationToken ct);
    Task<List<UserRoleRow>> GetUserRolesAsync(CancellationToken ct);
    Task<PermissionReportLookupDto> GetLookupsAsync(CancellationToken ct);
}