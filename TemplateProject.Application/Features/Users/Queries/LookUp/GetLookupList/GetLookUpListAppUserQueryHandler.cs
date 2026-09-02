using MediatR;
using QrAssignment.Application.Features.Users.Queries.LookUp.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Queries.LookUp.GetLookupList
{
    public class GetLookUpListAppUserQueryHandler : IRequestHandler<GetLookUpListAppUserQuery, Result<List<AppUserLookUpListItemDto>>>
    {
        private readonly IAppUserRepository _appUserRepository;

        public GetLookUpListAppUserQueryHandler(IAppUserRepository appUserRepository)
        {
            _appUserRepository = appUserRepository;
        }

        public async Task<Result<List<AppUserLookUpListItemDto>>> Handle(GetLookUpListAppUserQuery request, CancellationToken cancellationToken)
        {
            var result = await _appUserRepository.GetLookUpList(cancellationToken);

            return Result.Success(result);
        }

    }
}
