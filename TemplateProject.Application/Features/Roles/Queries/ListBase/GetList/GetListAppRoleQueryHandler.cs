using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Roles.Queries.GetList;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Queries.ListBase.GetList
{
    // Handler
    public sealed class GetListAppRoleQueryHandler : IRequestHandler<GetListAppRoleQuery, Result<Paginate<RoleListItemDto>>>
    {
        private readonly IAppRoleRepository _appRoleRepository;

        public GetListAppRoleQueryHandler(IAppRoleRepository appRoleRepository)
        {
            _appRoleRepository = appRoleRepository;
        }

        public async Task<Result<Paginate<RoleListItemDto>>> Handle(GetListAppRoleQuery request, CancellationToken cancellationToken)
        {
            var result = await _appRoleRepository.GetDtoListAsync(request, cancellationToken);
             
            return Result.Success(result);
        }
    }
}