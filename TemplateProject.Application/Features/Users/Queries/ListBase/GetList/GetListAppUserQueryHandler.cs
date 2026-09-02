using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Users.Queries.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Queries.ListBase.GetList
{
    public class GetListAppUserQueryHandler
        : IRequestHandler<GetListAppUserQuery, Result<Paginate<AppUserListItemDto>>>
    {
        private readonly IAppUserRepository _appUserRepository;

        public GetListAppUserQueryHandler(IAppUserRepository appUserRepository)
            => _appUserRepository = appUserRepository;

        public async Task<Result<Paginate<AppUserListItemDto>>> Handle(GetListAppUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _appUserRepository.GetDtoListAsync(request, cancellationToken);
            return Result.Success(result);
        }
    }
}
