// Application/Features/Permission/Queries/GetPagePermissionsForPage/GetPagePermissionsForPageQueryHandler.cs
using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.PagePermissions.DTOs;
using QrAssignment.Application.Features.PagePermissions.Queries;
using QrAssignment.Application.Repositories; // IUnitOfWork / IGenericRepository konumunuza göre düzeltin
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Permission.Queries.GetPagePermissionsForPage
{
    public sealed class GetPagePermissionsForPageQueryHandler
        : IRequestHandler<GetPagePermissionsForPageQuery, Result<List<PagePermissionAssignmentDto>>>
    { 
        private readonly IPageRepository _pageRepository;
        private readonly IPagePermissionRepository _pagePermissionRepository;

        public GetPagePermissionsForPageQueryHandler(IPageRepository pageRepository, IPagePermissionRepository pagePermissionRepository)
        { 
            _pageRepository = pageRepository;
            _pagePermissionRepository = pagePermissionRepository;
        }

        public async Task<Result<List<PagePermissionAssignmentDto>>> Handle(
            GetPagePermissionsForPageQuery request, CancellationToken cancellationToken)
        {
            var page = await _pageRepository.GetPageByKeyAsync(request.PageKey,cancellationToken);

            if (page is null)
                return Result.Failure<List<PagePermissionAssignmentDto>>(
                    new Error("Page.NotFound", $"'{request.PageKey}' anahtarına sahip sayfa bulunamadı."));
            var assigtmentlist = await _pagePermissionRepository.GetPagePermissionList(page.Id, cancellationToken);

            var result = assigtmentlist.Select(p => new PagePermissionAssignmentDto(
                    p.UserId,
                    p.User != null ? p.User.FullName : null,
                    p.RoleId,
                    p.Role != null ? p.Role.Name : null,
                    (int)p.PermissionValue)).ToList();

            return Result.Success(result);
        }
    }
}