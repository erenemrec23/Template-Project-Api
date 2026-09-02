using MediatR;
using QrAssignment.Application.Features.Users.Queries.DTOs;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Queries.FormBase.GetPassivedById
{
    public class GetPassivedByIdAppUserQueryHandler : IRequestHandler<GetPassivedByIdAppUserQuery, Result<AppUserItemDto>>
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly IAppLocalizer _localizer;

        public GetPassivedByIdAppUserQueryHandler(IAppUserRepository appUserRepository, IAppLocalizer localizer)
        {
            _appUserRepository = appUserRepository;
            _localizer = localizer;
        }

        public async Task<Result<AppUserItemDto>> Handle(GetPassivedByIdAppUserQuery request, CancellationToken cancellationToken)
        {
            if (request.Id is null)
                return Result.Failure<AppUserItemDto>(new Error("Error.UserNotFound", _localizer["Error.UserNotFound"]));

            var dto = await _appUserRepository.GetPassivedDtoByIdAsync(request.Id.Value, cancellationToken);
            if (dto is null) 
                return Result.Failure<AppUserItemDto>(new Error("Error.UserNotFound", _localizer["Error.UserNotFound"]));

            return Result.Success(dto);
        }
    }
}
