using MediatR;
using QrAssignment.Application.Features.Users.Queries.LookUp.DTOs;
using QrAssignment.Application.Features.Users.Queries.LookUp.GetLookupList;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Queries.LookUp.GetLookUpListAppUserAssignedRoleId
{
    public class GetLookUpListAppUserAssignedRoleIdQueryHandler : IRequestHandler<GetLookUpListAppUserAssignedRoleIdQuery, Result<List<Guid>>>
    {
        private readonly IAppUserRepository _appUserRepository;

        public GetLookUpListAppUserAssignedRoleIdQueryHandler(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        public async Task<Result<List<Guid>>> Handle(GetLookUpListAppUserAssignedRoleIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _appUserRepository.GetAssignedRoleIdsAsync(request.UserId.Value, cancellationToken);

            return Result.Success(result);
        }

    }
}
