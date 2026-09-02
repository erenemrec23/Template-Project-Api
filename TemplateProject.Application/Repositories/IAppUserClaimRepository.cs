using QrAssignment.Application.Features.Permission.Queries.GetByUserId;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Application.Repositories
{
    public interface IAppUserClaimRepository
    {
        Task<List<PermissionUserPageItemDto>> GetUserWithPermissionsAsync(Guid? userId, CancellationToken cancellationToken = default);
        Task<List<PermissionUserPageItemDto>> GetEffectivePagePermissionsAsync(
    Guid? userId, CancellationToken cancellationToken = default);
    }
}
