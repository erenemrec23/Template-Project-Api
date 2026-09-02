using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Roles.Queries.LookUp.GetRoleLookUpWithPermission;
using QrAssignment.Application.Features.Users.Queries.LookUp.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Queries.LookUp.GetRoleLookUp
{
    public class GetRoleLookUpQueryHandler
        : IRequestHandler<GetRoleLookUpQuery, Result<List<RoleLookUpListItemDto>>>
    {
        private readonly IAppRoleRepository _appRoleRepository;

        public GetRoleLookUpQueryHandler(IAppRoleRepository appRoleRepository)
        {
            _appRoleRepository = appRoleRepository;
        }

        public async Task<Result<List<RoleLookUpListItemDto>>> Handle(
            GetRoleLookUpQuery request,
            CancellationToken cancellationToken)
        {
            var result = await _appRoleRepository.GetDtoLookUpListAsync(
                new PageRequestBaseDto() { PageSize = int.MaxValue },
                cancellationToken);

            return Result.Success(result.Items.ToList());
        }
    }
}