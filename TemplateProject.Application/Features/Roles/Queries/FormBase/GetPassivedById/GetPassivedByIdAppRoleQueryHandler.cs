using MediatR;
using QrAssignment.Application.Features.Roles.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Queries.FormBase.GetPassivedById
{
    public class GetPassivedByIdAppRoleQueryHandler : IRequestHandler<GetPassivedByIdAppRoleQuery, Result<RoleItemDto>>
    {
        private readonly IAppRoleRepository _appRoleRepository;
        public GetPassivedByIdAppRoleQueryHandler(IAppRoleRepository appRoleRepository)
            => _appRoleRepository = appRoleRepository;

        public async Task<Result<RoleItemDto>> Handle(GetPassivedByIdAppRoleQuery request, CancellationToken cancellationToken)
        {
            var result = await _appRoleRepository.GetPassivedDtoByIdAsync(request.Id!.Value, cancellationToken);
            return Result.Success(result);
        }
    }
}