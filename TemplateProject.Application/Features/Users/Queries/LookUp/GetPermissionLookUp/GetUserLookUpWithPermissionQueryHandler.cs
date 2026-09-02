using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Users.Queries.LookUp.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Queries.LookUp.GetPermissionLookUp
{
    public class GetUserLookUpWithPermissionQueryHandler
        : IRequestHandler<GetUserLookUpWithPermissionQuery, Result<Paginate<PermissionLookUpListItemDto>>>
    {
        private readonly IAppUserRepository _appUserRepository;

        public GetUserLookUpWithPermissionQueryHandler(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

         
        public async Task<Result<Paginate<PermissionLookUpListItemDto>>> Handle(
    GetUserLookUpWithPermissionQuery request, CancellationToken ct)
    => Result.Success(await _appUserRepository.GetUserLookUpWithPermissionAsync(request, ct));
    }
}