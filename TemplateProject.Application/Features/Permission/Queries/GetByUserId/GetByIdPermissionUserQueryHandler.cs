using MediatR;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Permission.Queries.GetByUserId
{
     
    public class GetByIdPermissionUserQueryHandler : IRequestHandler<GetByIdPermissionUserQuery, Result<PermissionUserItemDto>>
    {
        private readonly IAppUserClaimRepository  _appUserClaimRepository;

        public GetByIdPermissionUserQueryHandler(IAppUserClaimRepository  appUserClaimRepository)
        {
            _appUserClaimRepository = appUserClaimRepository;
        }

        public async Task<Result<PermissionUserItemDto>> Handle(GetByIdPermissionUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _appUserClaimRepository.GetUserWithPermissionsAsync(request.UserId, cancellationToken);

            return Result.Success(new PermissionUserItemDto()
            {
                PagePermissionList = result,
                UserId = request.UserId
            });
        }
    }
}
