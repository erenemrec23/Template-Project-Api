using QrAssignment.Application.Features.Permission.Commands.Update; // PermissionUserUpdateDto

namespace QrAssignment.Application.Services
{
    /// <summary>
    /// Sayfa/grup yetkilerini senkronize eder. Coka-cok: ayni yetki seti verilen tum
    /// kullanicilara/rollere uygulanir; her hedef icin kendi satirlari full-replace edilir.
    /// </summary>
    public interface IPermissionSyncService
    {
        Task SyncUsersPermissionsAsync(
            IReadOnlyCollection<Guid> userIds,
            IEnumerable<PermissionUserUpdateDto> permissions,
            CancellationToken ct = default);

        Task SyncRolesPermissionsAsync(
            IReadOnlyCollection<Guid> roleIds,
            IEnumerable<PermissionUserUpdateDto> permissions,
            CancellationToken ct = default);
    }
}