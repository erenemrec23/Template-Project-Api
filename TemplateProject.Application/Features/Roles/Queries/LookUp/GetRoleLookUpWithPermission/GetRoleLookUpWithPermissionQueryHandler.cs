using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Users.Queries.LookUp.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Queries.LookUp.GetRoleLookUpWithPermission
{
    public class GetRoleLookUpWithPermissionQueryHandler
        : IRequestHandler<GetRoleLookUpWithPermissionQuery, Result<Paginate<PermissionLookUpListItemDto>>>
    {
        private readonly IAppUserRepository _appUserRepository;

        public GetRoleLookUpWithPermissionQueryHandler(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        public async Task<Result<Paginate<PermissionLookUpListItemDto>>> Handle(
            GetRoleLookUpWithPermissionQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _appUserRepository.GetRoleLookUpWithPermissionAsync(
                request,
                cancellationToken);

            return Result.Success(result);
        }
    }
}