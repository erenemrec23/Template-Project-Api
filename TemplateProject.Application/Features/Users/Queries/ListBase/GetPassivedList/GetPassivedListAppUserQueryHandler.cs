using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Users.Queries.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Queries.ListBase.GetPassivedList
{
    public class GetPassivedListAppUserQueryHandler
        : IRequestHandler<GetPassivedListAppUserQuery, Result<Paginate<AppUserListItemDto>>>
    {
        private readonly IAppUserRepository _appUserRepository;

        public GetPassivedListAppUserQueryHandler(IAppUserRepository appUserRepository)
            => _appUserRepository = appUserRepository;

        public async Task<Result<Paginate<AppUserListItemDto>>> Handle(GetPassivedListAppUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _appUserRepository.GetPassivedDtoListAsync(request, cancellationToken);
            return Result.Success(result);
        }
    }
}
