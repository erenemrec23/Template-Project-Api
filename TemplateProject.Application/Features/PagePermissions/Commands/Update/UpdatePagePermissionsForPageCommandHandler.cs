// Application/Features/Permission/Commands/UpdatePagePermissionsForPage/UpdatePagePermissionsForPageCommandHandler.cs
using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using QrAssignment.Application.Features.Permission.Commands.Update;
using QrAssignment.Application.Features.Roles.Commands.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.PagePermissions.Commands.Update
{
    public sealed class UpdatePagePermissionsForPageCommandHandler
        : IRequestHandler<UpdatePagePermissionsForPageCommand, Result>


    { 
        private readonly IPageRepository _pageRepository;
        private readonly IPagePermissionRepository _permissionRepository;
        private readonly IAppRoleRepository _appRoleRepository;
        private readonly IAppUserRepository _appUserRepository;
        private readonly IPermissionSyncService _permissionSyncService;

        public UpdatePagePermissionsForPageCommandHandler(IPageRepository pageRepository, 
            IPagePermissionRepository permissionRepository, 
            IAppRoleRepository roleRepository,
            IAppUserRepository appUserRepository,
            IPermissionSyncService permissionSyncService)
        { 
            _pageRepository = pageRepository;
            _permissionRepository = permissionRepository;
            _appRoleRepository = roleRepository;
            _appUserRepository = appUserRepository;
            _permissionSyncService = permissionSyncService;
        }

        public async Task<Result> Handle(
            UpdatePagePermissionsForPageCommand request, CancellationToken cancellationToken)
        {
            // Defense in depth: pipeline'da bypass edilse bile PageKey gerçekten var mı diye burada da doğrula
            var page = await _pageRepository.GetPageByKeyAsync(request.PageKey, cancellationToken);

            if (page is null)
                return Result.Failure(
                    new Error("Page.NotFound", $"'{request.PageKey}' anahtarına sahip sayfa bulunamadı."));


            if (request.RoleId.HasValue)
            {
                var lis = new List<Guid>();
                lis.Add(request.RoleId.Value);
                await _permissionSyncService.SyncRolesPermissionsAsync(lis, request.Permissions.Select(s => new PermissionUserUpdateDto(s.PageName, s.GroupKey,s.PermissionValue)), cancellationToken);
            
            }
            else
                if (request.UserId.HasValue)
                {

                    await _appUserRepository.SyncUserPermissionsAsync(request.UserId.Value, request.Permissions.Select(s => new PermissionUserUpdateDto() { GroupKey = s.GroupKey,  PageName = s.PageName, PermissionValue = s.PermissionValue }), cancellationToken);
                }



            return Result.Success();
        }

    }
}