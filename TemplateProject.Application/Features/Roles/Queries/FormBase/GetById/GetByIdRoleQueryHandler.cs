using MediatR;
using QrAssignment.Application.Features.Roles.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Roles.Queries.FormBase.GetById
{
    public class GetByIdAppRoleQueryHandler : IRequestHandler<GetByIdRoleQuery, Result<RoleItemDto>>
    {
        private readonly IAppRoleRepository _appRoleRepository;
        public GetByIdAppRoleQueryHandler(IAppRoleRepository appRoleRepository)
            => _appRoleRepository = appRoleRepository;

        public async Task<Result<RoleItemDto>> Handle(GetByIdRoleQuery request, CancellationToken cancellationToken)
        {
            var result = await _appRoleRepository.GetDtoByIdAsync(request.Id!.Value, cancellationToken);
            return Result.Success(result);
        }
    }
}