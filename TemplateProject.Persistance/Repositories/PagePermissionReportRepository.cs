using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.Features.Permission.Queries.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared.PagePermission;
using QrAssignment.Persistance.Context;

namespace QrAssignment.Infrastructure.Repositories;

public sealed class PagePermissionReportRepository : IPagePermissionReportRepository
{
    private readonly AppDbContext _context;
    public PagePermissionReportRepository(AppDbContext context) => _context = context;

    public Task<List<PermissionPageRow>> GetPagesAsync(CancellationToken ct) =>
        _context.Pages.AsNoTracking()
            .OrderBy(p => p.MenuGroup != null ? p.MenuGroup.Order : int.MaxValue)
            .ThenBy(p => p.Order)
            .Select(p => new PermissionPageRow(
                p.Id, p.PageKey, p.Key, p.MenuGroupId,
                p.MenuGroup != null ? p.MenuGroup.Key : null,
                p.ShowInMenu))
            .ToListAsync(ct);

    public Task<List<PermissionSourceRow>> GetSourceRowsAsync(CancellationToken ct) =>
        _context.PagePermissions.AsNoTracking()
            .Where(pp =>
                (pp.UserId != null && pp.User != null && !pp.User.IsPassived) ||
                (pp.RoleId != null && pp.Role != null && !pp.Role.IsPassived))
            .Select(pp => new PermissionSourceRow(
                pp.UserId != null ? PermissionOwnerType.User : PermissionOwnerType.Role,
                pp.UserId != null ? pp.UserId.Value : pp.RoleId!.Value,
                pp.UserId != null ? pp.User!.FullName : pp.Role!.Name,
                pp.PageId, pp.MenuGroupId, pp.PermissionValue))
            .ToListAsync(ct);

    public Task<List<UserRoleRow>> GetUserRolesAsync(CancellationToken ct) =>
        _context.Set<AppUserRole>().AsNoTracking()
            .Where(ur => ur.AppUserId != null && ur.AppRoleId != null
                      && ur.AppUser != null && !ur.AppUser.IsPassived
                      && ur.AppRole != null && !ur.AppRole.IsPassived)
            .Select(ur => new UserRoleRow(ur.AppUserId!.Value, ur.AppUser!.FullName, ur.AppRoleId!.Value, ur.AppRole!.Name))
            .ToListAsync(ct);

    public async Task<PermissionReportLookupDto> GetLookupsAsync(CancellationToken ct) => new()
    {
        Users = await _context.AppUsers.AsNoTracking()
            .Where(u => !u.IsPassived)
            .OrderBy(u => u.FullName)
            .Select(u => new LookupItem<Guid>(u.Id, u.FullName))
            .ToListAsync(ct),

        Roles = await _context.AppRoles.AsNoTracking()
            .Where(r => !r.IsPassived)
            .OrderBy(r => r.Name)
            .Select(r => new LookupItem<Guid>(r.Id, r.Name))
            .ToListAsync(ct),

        MenuGroups = await _context.MenuGroups.AsNoTracking()
            .OrderBy(g => g.Order)
            .Select(g => new LookupItem<int>(g.Id, g.Key))
            .ToListAsync(ct),

        Pages = await GetPagesAsync(ct)
    };
}