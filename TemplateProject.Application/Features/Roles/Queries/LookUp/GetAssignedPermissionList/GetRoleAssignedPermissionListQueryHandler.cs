using MediatR;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Queries.GetAssignedPermissionList
{
    public class GetRoleAssignedPermissionListQueryHandler
        : IRequestHandler<GetRoleAssignedPermissionListQuery, Result<RolePermissionDto>>
    {
        private readonly IAppRoleRepository _appRoleRepository;
        public GetRoleAssignedPermissionListQueryHandler(IAppRoleRepository appRoleRepository)
            => _appRoleRepository = appRoleRepository;

        public async Task<Result<RolePermissionDto>> Handle(
            GetRoleAssignedPermissionListQuery request, CancellationToken cancellationToken)
        {
            var result = await _appRoleRepository
                .GetAssignedPermissionListDtoAsync(request.RoleId.Value, cancellationToken);

            return Result.Success(new RolePermissionDto()
            {
                PagePermissionList = result,
                RoleId = request.RoleId.Value,
            });
        }
    }
}