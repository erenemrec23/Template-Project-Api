using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Roles.DTOs;
using QrAssignment.Application.Features.Roles.Queries.GetList;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Queries.ListBase.GetPassivedList
{
    public class GetPassivedListAppRoleQueryHandler : IRequestHandler<GetPassivedListAppRoleQuery, Result<Paginate<RoleListItemDto>>>
    {
        private readonly IAppRoleRepository _appRoleRepository;
        public GetPassivedListAppRoleQueryHandler(IAppRoleRepository appRoleRepository)
            => _appRoleRepository = appRoleRepository;

        public async Task<Result<Paginate<RoleListItemDto>>> Handle(GetPassivedListAppRoleQuery request, CancellationToken cancellationToken)
        {
            var result = await _appRoleRepository.GetPassivedDtoListAsync(request, cancellationToken);
            return Result.Success(result);
        }
    }
}