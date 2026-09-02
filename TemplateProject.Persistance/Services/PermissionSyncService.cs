using QrAssignment.Application.Features.Permission.Commands.Update; // PermissionUserUpdateDto
using QrAssignment.Application.Interfaces;                          // ITenantIdService (namespace'i dogrula)
using QrAssignment.Application.Repositories;                        // IPagePermissionRepository
using QrAssignment.Domain.Entity.App;                              // PagePermission
using QrAssignment.Domain.Shared.PagePermission;                   // PageAccessFlags (namespace'i dogrula)

namespace QrAssignment.Application.Services
{
    internal sealed class PermissionSyncService : IPermissionSyncService
    {
        private readonly IPagePermissionRepository _pagePermissionRepository;
        private readonly ITenantIdService _tenantIdService;

        public PermissionSyncService(IPagePermissionRepository pagePermissionRepository, ITenantIdService tenantIdService)
        {
            _pagePermissionRepository = pagePermissionRepository;
            _tenantIdService = tenantIdService;
        }

        public async Task SyncUsersPermissionsAsync(
            IReadOnlyCollection<Guid> userIds,
            IEnumerable<PermissionUserUpdateDto> permissions,
            CancellationToken ct = default)
        {
            if (userIds is null || userIds.Count == 0) return;

            var (pageItems, groupItems, tenantId) = Prepare(permissions);

            // Anahtar -> Id cozumleri tum hedefler icin ORTAK, bir kez yapilir
            var pageMap = await _pagePermissionRepository.GetPageIdsByKeysAsync(pageItems.Select(p => p.PageName!), ct);
            var groupMap = await _pagePermissionRepository.GetMenuGroupIdsByKeysAsync(groupItems.Select(p => p.GroupKey!), ct);

            // Tum hedeflerin mevcut satirlari tek sorguda cekilip hedefe gore gruplanir
            var pagesByUser = (await _pagePermissionRepository.GetUserPagePermissionRowsAsync(userIds, ct)).ToLookup(x => x.UserId!.Value);
            var groupsByUser = (await _pagePermissionRepository.GetUserGroupPermissionRowsAsync(userIds, ct)).ToLookup(x => x.UserId!.Value);

            foreach (var userId in userIds.Distinct())
            {
                SyncPageRows(pagesByUser[userId].ToList(), pageItems, pageMap,
                    (pageId, value) => PagePermission.ForUser(userId, pageId, value, tenantId));

                SyncGroupRows(groupsByUser[userId].ToList(), groupItems, groupMap,
                    (groupId, value) => PagePermission.ForUserGroup(userId, groupId, value, tenantId));
            }
            // SaveChanges YOK — UnitOfWorkBehavior commit eder
        }

        public async Task SyncRolesPermissionsAsync(
            IReadOnlyCollection<Guid> roleIds,
            IEnumerable<PermissionUserUpdateDto> permissions,
            CancellationToken ct = default)
        {
            if (roleIds is null || roleIds.Count == 0) return;

            var (pageItems, groupItems, tenantId) = Prepare(permissions);

            var pageMap = await _pagePermissionRepository.GetPageIdsByKeysAsync(pageItems.Select(p => p.PageName!), ct);
            var groupMap = await _pagePermissionRepository.GetMenuGroupIdsByKeysAsync(groupItems.Select(p => p.GroupKey!), ct);

            var pagesByRole = (await _pagePermissionRepository.GetRolePagePermissionRowsAsync(roleIds, ct)).ToLookup(x => x.RoleId!.Value);
            var groupsByRole = (await _pagePermissionRepository.GetRoleGroupPermissionRowsAsync(roleIds, ct)).ToLookup(x => x.RoleId!.Value);

            foreach (var roleId in roleIds.Distinct())
            {
                SyncPageRows(pagesByRole[roleId].ToList(), pageItems, pageMap,
                    (pageId, value) => PagePermission.ForRole(roleId, pageId, value, tenantId));

                SyncGroupRows(groupsByRole[roleId].ToList(), groupItems, groupMap,
                    (groupId, value) => PagePermission.ForRoleGroup(roleId, groupId, value, tenantId));
            }
        }
         
        private (List<PermissionUserUpdateDto> pageItems, List<PermissionUserUpdateDto> groupItems, Guid? tenantId) Prepare(
            IEnumerable<PermissionUserUpdateDto> permissions)
        {
            var incoming = (permissions ?? Enumerable.Empty<PermissionUserUpdateDto>()).ToList();

            var pageItems = incoming.Where(p => !string.IsNullOrEmpty(p.PageName)).ToList();
            var groupItems = incoming.Where(p => string.IsNullOrEmpty(p.PageName) && !string.IsNullOrEmpty(p.GroupKey)).ToList();

            return (pageItems, groupItems, _tenantIdService.GetTenantId());
        }

        private void SyncPageRows(
            List<PagePermission> current,
            List<PermissionUserUpdateDto> items,
            IReadOnlyDictionary<string, int> pageMap,
            Func<int, PageAccessFlags, PagePermission> factory)
        {
            var toRemove = new List<PagePermission>();

            foreach (var p in items)
            {
                if (!pageMap.TryGetValue(p.PageName!, out var pageId)) continue;
                var existing = current.FirstOrDefault(x => x.PageId == pageId);

                // Yetki kaldirildi (0/null) -> varsa sil, yoksa dokunma
                if (p.PermissionValue <= 0)
                {
                    if (existing is not null) toRemove.Add(existing);
                    continue;
                }

                var value = (PageAccessFlags)p.PermissionValue;
                if (existing is null)
                    _pagePermissionRepository.AddAsync(factory(pageId, value));
                else
                {
                    existing.PermissionValue = value;
                    existing.IsPassived = false;
                }
            }

            if (toRemove.Count > 0) _pagePermissionRepository.DeleteRange(toRemove);
        }

        // --- GRUP satirlari (hedef-agnostik: user ya da role) ---
        private void SyncGroupRows(
            List<PagePermission> current,
            List<PermissionUserUpdateDto> items,
            IReadOnlyDictionary<string, short> groupMap,
            Func<short, PageAccessFlags, PagePermission> factory)
        {
            var toRemove = new List<PagePermission>();

            foreach (var p in items)
            {
                if (!groupMap.TryGetValue(p.GroupKey!, out var groupId)) continue;
                var existing = current.FirstOrDefault(x => x.MenuGroupId == groupId);

                if (p.PermissionValue <= 0)
                {
                    if (existing is not null) toRemove.Add(existing);
                    continue;
                }

                var value = (PageAccessFlags)p.PermissionValue;
                if (existing is null)
                    _pagePermissionRepository.AddAsync(factory(groupId, value));
                else
                {
                    existing.PermissionValue = value;
                    existing.IsPassived = false;
                }
            }

            if (toRemove.Count > 0) _pagePermissionRepository.DeleteRange(toRemove);
        }
    }
}