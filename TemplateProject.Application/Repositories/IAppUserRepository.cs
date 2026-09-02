using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Permission.Commands.Update;
using QrAssignment.Application.Features.Permission.Queries.GetByUserId;
using QrAssignment.Application.Features.Roles.Queries.LookUp.GetRoleLookUpWithPermission;
using QrAssignment.Application.Features.Users.DTOs;
using QrAssignment.Application.Features.Users.Queries.DTOs;
using QrAssignment.Application.Features.Users.Queries.LookUp.DTOs;
using QrAssignment.Application.Features.Users.Queries.LookUp.GetPermissionLookUp;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Repositories
{
    public interface IAppUserRepository
    {
        // --- Ortak okuma yuzeyi (AppRole ile ayni) ---
        Task<Paginate<AppUserListItemDto>> GetDtoListAsync(PageRequestBaseDto request, CancellationToken ct = default);
        Task<Paginate<AppUserListItemDto>> GetPassivedDtoListAsync(PageRequestBaseDto request, CancellationToken ct = default);
        Task<List<AppUserListItemExcelDto>> GetExportListAsync(PageRequestBaseDto request, CancellationToken ct = default);
        Task<AppUserItemDto?> GetDtoByIdAsync(Guid id, CancellationToken ct = default);
        Task<AppUserItemDto?> GetPassivedDtoByIdAsync(Guid id, CancellationToken ct = default);
        Task BulkDeleteAsync(List<Guid> ids, CancellationToken ct);

        // --- Excel Uniqueness & Bulk Helpers ---
        Task<List<string>> GetExistingUserNamesAsync(List<string> userNames, CancellationToken ct = default);
        Task<List<string>> GetExistingEmailsAsync(List<string> emails, CancellationToken ct = default);

        // --- User'a ozel ---
        Task<List<AppUserLookUpListItemDto>> GetLookUpList(CancellationToken ct);
        Task<AppUser?> GetByIdWithRefreshTokenAsync(Guid id, CancellationToken ct = default);
        Task<AppUser?> GetByEmailWithRefreshTokenAsync(string email, CancellationToken ct = default);
        Task<AppUser?> GetByEmailWithRefreshTokenAsync(Guid userid, string email, CancellationToken ct = default);

        // --- Role & Permission Mappings (AppRoleParity) ---
        Task<List<Guid>> GetAssignedRoleListDtoAsync(Guid userId, CancellationToken ct = default);
        Task SyncAssignedRolesAsync(Guid userId, IEnumerable<Guid> roleIds, CancellationToken ct = default);
        Task<List<PermissionUserPageItemDto>> GetAssignedPermissionListDtoAsync(Guid userId, CancellationToken ct = default);

        Task BulkSetActiveByIds(List<Guid> ids, CancellationToken ct);
        Task SetActiveAsync(Guid id, CancellationToken ct);
        Task<AppUser?> GetPassivedByIdAsync(Guid id, CancellationToken ct = default);

        Task DeleteById(Guid id, CancellationToken ct);

        Task<AppUser?> GetByEmailForRememberPasswordAsync(string email, CancellationToken ct = default);

        Task SyncUserPermissionsAsync(Guid userId, IEnumerable<PermissionUserUpdateDto> permissions, CancellationToken ct = default);


        Task<Paginate<PermissionLookUpListItemDto>> GetRoleLookUpWithPermissionAsync(
    GetRoleLookUpWithPermissionQuery request, CancellationToken ct = default);
        Task<Paginate<PermissionLookUpListItemDto>> GetUserLookUpWithPermissionAsync(
    GetUserLookUpWithPermissionQuery request, CancellationToken ct = default);

        Task<List<Guid>> GetAssignedRoleIdsAsync(Guid userId, CancellationToken ct);

    }
}