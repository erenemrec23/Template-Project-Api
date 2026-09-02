using MediatR;
using QrAssignment.Application.Features.Permission.Queries.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;
using QrAssignment.Domain.Shared.PagePermission;

namespace QrAssignment.Application.Features.Permission.Queries.GetPermissionReport;

public sealed class GetPermissionReportQueryHandler
    : IRequestHandler<GetPermissionReportQuery, Result<List<PermissionReportItemDto>>>
{
    private readonly IPagePermissionReportRepository _repo;

    public GetPermissionReportQueryHandler(IPagePermissionReportRepository repo) => _repo = repo;

    public async Task<Result<List<PermissionReportItemDto>>> Handle(GetPermissionReportQuery q, CancellationToken ct)
    {
        var pages = await _repo.GetPagesAsync(ct);
        var sources = await _repo.GetSourceRowsAsync(ct);
        var userRoles = await _repo.GetUserRolesAsync(ct);

        var pagesByGroup = pages
            .Where(p => p.MenuGroupId.HasValue)
            .GroupBy(p => p.MenuGroupId!.Value)
            .ToDictionary(g => g.Key, g => g.ToList());

        var groupKeys = pagesByGroup
            .ToDictionary(kv => kv.Key, kv => kv.Value[0].MenuGroupKey ?? kv.Key.ToString());

        var byOwner = sources
            .GroupBy(s => (s.OwnerType, s.OwnerId))
            .ToDictionary(g => g.Key, g => g.ToList());

        var rolesByUser = userRoles
            .GroupBy(r => r.UserId)
            .ToDictionary(g => g.Key, g => g.ToList());

        var result = new List<PermissionReportItemDto>();

        // ---- Kullanıcılar: doğrudan + grup + rol + rol-grup ----
        if (q.OwnerType is null or PermissionOwnerType.User)
        {
            var users = sources
                .Where(s => s.OwnerType == PermissionOwnerType.User)
                .Select(s => (Id: s.OwnerId, Name: s.OwnerName))
                .Concat(userRoles.Select(r => (Id: r.UserId, Name: r.UserName)))
                .GroupBy(x => x.Id)
                .Select(g => (Id: g.Key, Name: g.First().Name))
                .Where(u => q.UserId is null || u.Id == q.UserId);

            foreach (var (userId, userName) in users)
            {
                var roles = rolesByUser.GetValueOrDefault(userId) ?? [];

                if (q.RoleId is not null && !roles.Any(r => r.RoleId == q.RoleId))
                    continue;

                var acc = new Dictionary<int, Accum>();

                Accumulate(acc, byOwner.GetValueOrDefault((PermissionOwnerType.User, userId)), pagesByGroup, groupKeys,
                    pageSource: new PermissionSourceInfo("Direct", null, null),
                    groupSource: gk => new PermissionSourceInfo("Group", null, gk));

                foreach (var role in roles)
                    Accumulate(acc, byOwner.GetValueOrDefault((PermissionOwnerType.Role, role.RoleId)), pagesByGroup, groupKeys,
                        pageSource: new PermissionSourceInfo("Role", role.RoleName, null),
                        groupSource: gk => new PermissionSourceInfo("RoleGroup", role.RoleName, gk));

                Emit(result, PermissionOwnerType.User, userId, userName, acc, pages, q);
            }
        }

        // ---- Roller: yalnızca rolün kendi tanımları ----
        if (q.OwnerType is null or PermissionOwnerType.Role)
        {
            var roles = sources
                .Where(s => s.OwnerType == PermissionOwnerType.Role)
                .Select(s => (Id: s.OwnerId, Name: s.OwnerName))
                .Distinct()
                .Where(r => q.RoleId is null || r.Id == q.RoleId);

            foreach (var (roleId, roleName) in roles)
            {
                var acc = new Dictionary<int, Accum>();

                Accumulate(acc, byOwner.GetValueOrDefault((PermissionOwnerType.Role, roleId)), pagesByGroup, groupKeys,
                    pageSource: new PermissionSourceInfo("Direct", null, null),
                    groupSource: gk => new PermissionSourceInfo("Group", null, gk));

                Emit(result, PermissionOwnerType.Role, roleId, roleName, acc, pages, q);
            }
        }

        return Result.Success(result
            .OrderBy(r => r.OwnerType)
            .ThenBy(r => r.OwnerName)
            .ThenBy(r => r.MenuGroupKey)
            .ThenBy(r => r.Key)
            .ToList());
    }

    private sealed class Accum
    {
        public PageAccessFlags Value;
        public List<PermissionSourceInfo> Sources = [];
    }

    private static void Accumulate(
        Dictionary<int, Accum> acc,
        List<PermissionSourceRow>? rows,
        Dictionary<short, List<PermissionPageRow>> pagesByGroup,
        Dictionary<short, string> groupKeys,
        PermissionSourceInfo pageSource,
        Func<string, PermissionSourceInfo> groupSource)
    {
        if (rows is null) return;

        foreach (var row in rows)
        {
            if (row.Value == PageAccessFlags.None) continue;

            if (row.PageId is int pageId)
            {
                Add(acc, pageId, row.Value, pageSource);
            }
            else if (row.MenuGroupId is short gid && pagesByGroup.TryGetValue(gid, out var groupPages))
            {
                var src = groupSource(groupKeys[gid]);
                foreach (var p in groupPages)
                    Add(acc, p.PageId, row.Value, src);
            }
        }
    }

    private static void Add(Dictionary<int, Accum> acc, int pageId, PageAccessFlags value, PermissionSourceInfo source)
    {
        if (!acc.TryGetValue(pageId, out var cur))
            acc[pageId] = cur = new Accum();

        cur.Value |= value;
        if (!cur.Sources.Contains(source))   // record → değer eşitliği
            cur.Sources.Add(source);
    }

    private static void Emit(
        List<PermissionReportItemDto> result,
        PermissionOwnerType ownerType, Guid ownerId, string ownerName,
        Dictionary<int, Accum> acc,
        List<PermissionPageRow> pages,
        GetPermissionReportQuery q)
    {
        foreach (var page in pages)
        {
            if (q.PageId is not null && page.PageId != q.PageId) continue;
            if (q.MenuGroupId is not null && page.MenuGroupId != q.MenuGroupId) continue;

            var hit = acc.TryGetValue(page.PageId, out var a);
            var value = hit ? a!.Value : PageAccessFlags.None;

            if (q.OnlyGranted && value == PageAccessFlags.None) continue;
            if (q.HasFlag is { } f && f != PageAccessFlags.None && (value & f) != f) continue;

            result.Add(new PermissionReportItemDto
            {
                OwnerType = ownerType,
                OwnerId = ownerId,
                OwnerName = ownerName,
                PageId = page.PageId,
                PageKey = page.PageKey,
                Key = page.Key,
                MenuGroupId = page.MenuGroupId,
                MenuGroupKey = page.MenuGroupKey,
                PermissionValue = (int)value,
                Sources = hit ? a!.Sources : []
            });
        }
    }
}