using MediatR;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Queries.LookUp.GetAssignedUserList
{
     
    public class GetRoleAssignedUserListQueryHandler : IRequestHandler<GetRoleAssignedUserListQuery, Result<List<Guid>>>
    {
        private readonly IAppRoleRepository _appRoleRepository;
        public GetRoleAssignedUserListQueryHandler(IAppRoleRepository appRoleRepository)
            => _appRoleRepository = appRoleRepository;

        public async Task<Result<List<Guid>>> Handle(GetRoleAssignedUserListQuery request, CancellationToken cancellationToken)
        {
            var result = await _appRoleRepository.GetAssignedUserListDtoAsync(request.RoleId.Value, cancellationToken);
            return Result.Success(result);
        }
    }
}
