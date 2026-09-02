// Application/Features/Permission/Commands/UpdatePagePermissionsForPage/UpdatePagePermissionsForPageCommandHandler2.cs
using MediatR;
using QrAssignment.Application.Features.PagePermissions.Commands.Update;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.PagePermissions.Commands.Update2
{
    public sealed class UpdatePagePermissionsForPageCommandHandler2
        : IRequestHandler<UpdatePagePermissionsForPageCommand2, Result>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IPagePermissionRepository _permissionRepository;

        public UpdatePagePermissionsForPageCommandHandler2(IPageRepository pageRepository, IPagePermissionRepository permissionRepository)
        {
            _pageRepository = pageRepository;
            _permissionRepository = permissionRepository;
        }

        public async Task<Result> Handle(
            UpdatePagePermissionsForPageCommand2 request, CancellationToken cancellationToken)
        {
            // Defense in depth: pipeline'da bypass edilse bile PageKey gerçekten var mı diye burada da doğrula
            var page = await _pageRepository.GetPageByKeyAsync(request.PageKey, cancellationToken);

            if (page is null)
                return Result.Failure(
                    new Error("Page.NotFound", $"'{request.PageKey}' anahtarına sahip sayfa bulunamadı."));



            var assignments = await _permissionRepository.GetPagePermissionList(page.Id, cancellationToken);


            _permissionRepository.DeleteRange(assignments);
            var newRows = request.Permissions.Select(a => new PagePermission
            {
                PageId = page.Id,
                //UserId = a.UserId,
                //RoleId = a.RoleId,
                PermissionValue = (Domain.Shared.PagePermission.PageAccessFlags)a.PermissionValue
            });

            await _permissionRepository.AddRangeAsync(newRows, cancellationToken);


            return Result.Success();
        }
    }
}