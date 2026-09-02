using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Repositories
{
    public interface IPagePermissionRepository : IGenericRepository<PagePermission>
    {
        Task<List<PagePermission>> GetPagePermissionList(int pageId, CancellationToken ct = default);

        Task<Dictionary<string, int>> GetPageIdsByKeysAsync(IEnumerable<string> pageKeys, CancellationToken ct = default);
        Task<Dictionary<string, short>> GetMenuGroupIdsByKeysAsync(IEnumerable<string> groupKeys, CancellationToken ct = default);

        // Mevcut satirlar (TRACKED donmeli — servis PermissionValue/IsPassived gunceller)
        Task<List<PagePermission>> GetUserPagePermissionRowsAsync(IReadOnlyCollection<Guid> userIds, CancellationToken ct = default);
        Task<List<PagePermission>> GetUserGroupPermissionRowsAsync(IReadOnlyCollection<Guid> userIds, CancellationToken ct = default);
        Task<List<PagePermission>> GetRolePagePermissionRowsAsync(IReadOnlyCollection<Guid> roleIds, CancellationToken ct = default);
        Task<List<PagePermission>> GetRoleGroupPermissionRowsAsync(IReadOnlyCollection<Guid> roleIds, CancellationToken ct = default);
    }
}
 