using MediatR;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Permission.Commands.Update
{
    internal sealed class UpdateUserPermissionCommandHandler
        : IRequestHandler<UpdateUserPermissionCommand, Result>
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly IAppLocalizer _localizer;

        public UpdateUserPermissionCommandHandler(
            IAppUserRepository appUserRepository,
            IAppLocalizer localizer)
        {
            _appUserRepository = appUserRepository;
            _localizer = localizer;
        }

        public async Task<Result> Handle(UpdateUserPermissionCommand request, CancellationToken ct)
        {
            if (!Guid.TryParse(request.UserId, out var userId))
                return Result.Failure(new Error("UserNotFound", _localizer["Error.UserNotFound"]));

            await _appUserRepository.SyncUserPermissionsAsync(userId, request.Permissions, ct);

            return Result.Success();
        }
    }
}