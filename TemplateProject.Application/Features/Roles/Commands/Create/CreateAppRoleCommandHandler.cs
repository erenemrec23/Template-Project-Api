using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.Permission.Commands.Update;
using QrAssignment.Application.Features.Roles.Commands.DTOs;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;
using QrAssignment.Domain.Shared.PagePermission;

namespace QrAssignment.Application.Features.Roles.Commands.Create
{
    public sealed class CreateAppRoleCommandHandler : IRequestHandler<CreateAppRoleCommand, Result>
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IAppRoleRepository _appRoleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppLocalizer _localizer;
        private readonly IPermissionSyncService _permissionSyncService;

        public CreateAppRoleCommandHandler(
            RoleManager<AppRole> roleManager,
            IAppRoleRepository appRoleRepository,
            IUnitOfWork unitOfWork,
            IAppLocalizer localizer,
            IPermissionSyncService permissionSyncService)
        {
            _roleManager = roleManager;
            _appRoleRepository = appRoleRepository;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _permissionSyncService = permissionSyncService;
        }

        public async Task<Result> Handle(CreateAppRoleCommand request, CancellationToken ct)
        {
            if (await _roleManager.RoleExistsAsync(request.Name))
                return Result.Failure(new Error(
                    "Error.RoleHasInserted",
                    string.Format(_localizer["Error.RoleHasInserted"], request.Name)));

            var role = new AppRole { Name = request.Name.Trim() };

            var createResult = await _roleManager.CreateAsync(role);
            if (!createResult.Succeeded)
            {
                var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
                return Result.Failure(new Error(
                    string.Format(_localizer["Error.CreateRole"], errors), ""));
            }

            await _unitOfWork.SaveChangesAsync(ct);   // role.Id üretilsin

            if (request.UserIds is { Count: > 0 })
                await _appRoleRepository.SyncAssignedUsersAsync(role.Id, request.UserIds, ct);

            // Sayfa yetkileri artık PagePermission tablosuna (Identity claim değil)
            await _permissionSyncService.SyncRolesPermissionsAsync(new List<Guid> { role.Id }, request.Permissions.Select(s => new PermissionUserUpdateDto(s.PageName, s.GroupKey, s.PermissionValue)).ToList(), ct);

            return Result.Success();
        }
    }
}