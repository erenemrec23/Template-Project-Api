
using QrAssignment.Application.Features.Menu.Queries.DTOs;
using QrAssignment.Domain.Entity.App;
namespace QrAssignment.Application.Repositories
{
    public interface IPageRepository
    {
        Task<List<PageCatalogItemDto>> GetCatalogAsync(string? pageKey = null, CancellationToken ct = default);
        Task<List<MenuGroupDto>> GetMenuAsync(CancellationToken ct = default);

        Task<Page> GetPageByKeyAsync(string pageKey, CancellationToken cancellationToken = default);
    }
}
 