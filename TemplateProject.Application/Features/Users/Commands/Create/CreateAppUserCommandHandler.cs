using MediatR;
using QrAssignment.Application.Repositories;   // IAppUserRepository (rol atama join'i icin)
using QrAssignment.Application.Services;        // IAuthService, IPermissionSyncService
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.Create
{
    internal sealed class CreateAppUserCommandHandler : IRequestHandler<CreateAppUserCommand, Result>
    {
        private readonly IAuthService _authService;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IPermissionSyncService _permissionSyncService;

        public CreateAppUserCommandHandler(
            IAuthService authService,
            IAppUserRepository appUserRepository,
            IPermissionSyncService permissionSyncService)
        {
            _authService = authService;
            _appUserRepository = appUserRepository;
            _permissionSyncService = permissionSyncService;
        }

        public async Task<Result> Handle(CreateAppUserCommand request, CancellationToken cancellationToken)
        {
            // CreateAsync olusturulan kullanicinin Id'sini doner.
            var userId = await _authService.CreateAsync(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password,
                cancellationToken);

            // Yetkiler artik servis uzerinden (coka-cok imza; tek kullanici tek elemanli liste).
            if (request.Permissions is not null)
            {
                await _permissionSyncService.SyncUsersPermissionsAsync(
                    new[] { userId }, request.Permissions, cancellationToken);
            }

            // Rol atama (AppUserRole join) repository'de kaliyor.
            if (request.RoleIds is not null)
            {
                await _appUserRepository.SyncAssignedRolesAsync(
                    userId, request.RoleIds, cancellationToken);
            }

            return Result.Success();
        }
    }
}