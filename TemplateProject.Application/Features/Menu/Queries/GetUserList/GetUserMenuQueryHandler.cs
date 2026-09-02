using MediatR; 
using QrAssignment.Application.Features.Menu.Queries.DTOs;
using QrAssignment.Application.Interfaces; 
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared.PagePermission;
using QrAssignment.Domain.Shared;
namespace QrAssignment.Application.Features.Menu.Queries.GetUserList
{
    public sealed class GetUserMenuQueryHandler
        : IRequestHandler<GetUserMenuQuery, Result<List<MenuGroupDto>>>
    {
        private readonly IPageRepository _pageRepository;
        private readonly ICurrentUserService _currentUser;

        public GetUserMenuQueryHandler(
            IPageRepository pageRepository,
            ICurrentUserService currentUser)
        {
            _pageRepository = pageRepository;
            _currentUser = currentUser;
        }

        public async Task<Result<List<MenuGroupDto>>> Handle(
            GetUserMenuQuery request, CancellationToken ct)
        {
            // 1) Kullanicinin sayfa yetkileri (token 'permissions' claim'inden):
            //    pageKey -> permissionValue (bit alani)
            var perms = _currentUser.GetPagePermissions();

            // 2) Tam menuyu al, SUNUCUDA View bitine gore filtrele.
            //    Yetkisiz sayfalar/gruplar hic network'e cikmaz.
            var fullMenu = await _pageRepository.GetMenuAsync(ct);

            const int view = (int)PageAccessFlags.View;

            bool CanView(string pageKey) =>
                perms.TryGetValue(pageKey, out var v) && (v & view) == view;

            var filtered = fullMenu
                // MenuGroupDto bir record ise 'with' calisir. Class ise asagidaki
                // NOT'a bak; children'i yeniden set etmen gerekir.
                .Select(group => group with
                {
                    Children = group.Children.Where(c => CanView(c.PageKey)).ToList()
                })
                .Where(group => group.Children.Count > 0)
                .ToList();

            return Result.Success(filtered);
        }
    }
}

/*
NOT — MenuGroupDto bir 'class' ise (record degilse) 'with' kullanilamaz.
O durumda Select yerine soyle yap:

    foreach (var group in fullMenu)
        group.Children = group.Children.Where(c => CanView(c.PageKey)).ToList();

    var filtered = fullMenu.Where(g => g.Children.Count > 0).ToList();

Ayrica child DTO'daki alan adi 'PageKey' degilse ( or. 'PageName') ona gore duzelt.
Frontend'de child.pageKey == token'daki pageName ile eslesiyordu; sunucuda da
ayni anahtari kullan.
*/