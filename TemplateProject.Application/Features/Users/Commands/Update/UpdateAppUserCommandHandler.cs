using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Application.Features.Users.Commands.Create;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;   // IAppUserRepository (rol atama join'i icin)
using QrAssignment.Application.Services;        // IPermissionSyncService
using QrAssignment.Domain.Entity.App;          // AppUser
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Commands.Update
{
    internal sealed class UpdateAppUserCommandHandler : IRequestHandler<UpdateAppUserCommand, Result>
    { 
        private readonly IAppUserRepository _appUserRepository;
        private readonly IPermissionSyncService _permissionSyncService; 
        private readonly IAuthService _authService;

        public UpdateAppUserCommandHandler( 
            IAppUserRepository appUserRepository,
            IPermissionSyncService permissionSyncService,  
            IAuthService authService)
        { 
            _appUserRepository = appUserRepository;
            _permissionSyncService = permissionSyncService; 
            _authService = authService;
        }

        public async Task<Result> Handle(UpdateAppUserCommand request, CancellationToken cancellationToken)
        {
            // CreateAsync olusturulan kullanicinin Id'sini doner.
            var userId = await _authService.UpdateAsync(
                request.Id.Value,
                request.FirstName,
                request.LastName,
                request.Email, 
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