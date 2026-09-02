using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using Microsoft.AspNetCore.Identity;
using QrAssignment.Application.Features.Roles.Commands.Update;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;

public sealed class UpdateAppRoleCommandHandler : IRequestHandler<UpdateAppRoleCommand, Result>
{
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IAppRoleRepository _appRoleRepository;
    private readonly IAppLocalizer _localizer;
    private readonly IPermissionSyncService _permissionSyncService; 
    public UpdateAppRoleCommandHandler(RoleManager<AppRole> roleManager,
        IAppRoleRepository appRoleRepository,
        IAppLocalizer localizer,
        IPermissionSyncService permissionSyncService)
    {
        _roleManager = roleManager;
        _appRoleRepository = appRoleRepository;
        _localizer = localizer;
        _permissionSyncService = permissionSyncService;
    }

    public async Task<Result> Handle(UpdateAppRoleCommand request, CancellationToken ct)
    {
        var role = await _roleManager.FindByIdAsync(request.Id.ToString());
        if (role is null)
            return Result.Failure(new Error(_localizer["Error.RoleNotFound"], "Error.RoleNotFound"));

        if (role.Name != request.Name && await _roleManager.RoleExistsAsync(request.Name))
            return Result.Failure(new Error(_localizer["Error.RoleDublicated"], "Error.RoleDublicated"));

        // 1) Personel
        await _appRoleRepository.SyncAssignedUsersAsync(role.Id, request.UserIds, ct);

        // 2) Sayfa yetkileri — PagePermission tablosu (tek metod: ekle/güncelle/sil) 
        //await _appRoleRepository.SyncRolePermissionsAsync(role.Id, request.Permissions, ct);
        await _permissionSyncService.SyncRolesPermissionsAsync(
                  new[] { role.Id }, request.Permissions, ct);


        // 3) Rol adı
        role.Name = request.Name;
        var updateResult = await _roleManager.UpdateAsync(role);
        if (!updateResult.Succeeded)
            return Result.Failure(new Error(
                 string.Format(_localizer["Error.RoleUCanNotUpdated"],
                 string.Join(", ", updateResult.Errors.Select(e => e.Description))), "Error.RoleUCanNotUpdated"));

        return Result.Success();
    }
}