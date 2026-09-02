using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Permission.Queries.GetByUserId;
using QrAssignment.Application.Features.Roles.Commands.DTOs;
using QrAssignment.Application.Features.Roles.DTOs;
using QrAssignment.Application.Features.Roles.Queries.GetList;
using QrAssignment.Application.Features.Users.Queries.LookUp.DTOs;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared.PagePermission;

namespace QrAssignment.Application.Repositories
{
        public interface IAppRoleRepository 
    {
        Task<Paginate<RoleListItemDto>> GetDtoListAsync(PageRequestBaseDto request, CancellationToken ct = default);
        Task<Paginate<RoleListItemDto>> GetPassivedDtoListAsync(PageRequestBaseDto request, CancellationToken ct = default);
        Task<List<RoleListItemExcelDto>> GetExportListAsync(PageRequestBaseDto request, CancellationToken ct = default);
        Task<RoleItemDto?> GetDtoByIdAsync(Guid id, CancellationToken ct = default);
        Task<RoleItemDto?> GetPassivedDtoByIdAsync(Guid id, CancellationToken ct = default);
        Task BulkDelete(List<Guid> ids, CancellationToken ct);
        Task Delete(Guid id, CancellationToken ct); 


        Task<List<AppRole>> GetByNamesAsync(List<string> names, CancellationToken ct);

        Task<List<Guid>> GetAssignedUserListDtoAsync(Guid roleId, CancellationToken ct = default);
        Task SyncAssignedUsersAsync(Guid roleId, IEnumerable<Guid> userIds, CancellationToken ct);

        Task<List<PermissionUserPageItemDto>> GetAssignedPermissionListDtoAsync(
    Guid roleId, CancellationToken cancellationToken);
        Task BulkSetActiveAsync(List<Guid> ids, CancellationToken ct);
        Task SetActiveAsync(Guid id, CancellationToken ct);
        Task<AppRole?> GetPassivedByIdAsync(Guid id, CancellationToken ct = default);

        Task DeleteById(Guid id, CancellationToken ct);

        Task SetPassiveById(Guid id, CancellationToken ct);
        Task BulkSetPassiveByIds(List<Guid> ids, CancellationToken ct);

    //    Task SyncRolePermissionsAsync(
    //Guid roleId,
    //IEnumerable<RolePagePermissionDto> permissions, 
    //CancellationToken ct = default);
        Task<Paginate<RoleLookUpListItemDto>> GetDtoLookUpListAsync(PageRequestBaseDto request, CancellationToken ct = default);
    }
}
 