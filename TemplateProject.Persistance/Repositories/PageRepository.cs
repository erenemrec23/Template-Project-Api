using Azure.Core;
using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.Features.Menu.Queries.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Context;

internal sealed class PageRepository : IPageRepository
{
    private readonly AppDbContext _context;
    public PageRepository(AppDbContext context) => _context = context;

    public Task<List<PageCatalogItemDto>> GetCatalogAsync(string? pageKey = null, CancellationToken ct = default)
    {
        var query = _context.Set<Page>()
            .AsNoTracking();

        // Filtreleme: pageKey dolu gelmişse ilgili sayfayı filtrele
        if (!string.IsNullOrWhiteSpace(pageKey))
        {
            query = query.Where(p => p.PageKey == pageKey);
        }

        return query
            .OrderBy(p => p.MenuGroupId).ThenBy(p => p.Order)
            .Select(p => new PageCatalogItemDto(
                p.PageKey,
                p.Key,
                p.MenuGroup != null ? p.MenuGroup.Key : null)) // grupsuz sayfa → null
            .ToListAsync(ct);
    }

    public Task<List<MenuGroupDto>> GetMenuAsync(CancellationToken ct = default)
    => _context.Set<MenuGroup>()
        .AsNoTracking()
        .OrderBy(g => g.Order)
        .Select(g => new MenuGroupDto(
            g.Key,
            g.Icon,
            g.Pages.Where(p => p.ShowInMenu)
                   .OrderBy(p => p.Order)
                   .Select(p => new MenuPageDto(p.PageKey, p.Key, p.Icon, p.Route))
                   .ToList()))
        .ToListAsync(ct);


    public Task<Page> GetPageByKeyAsync(string pageKey, CancellationToken cancellationToken = default)
    => _context.Set<Page>()
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.PageKey == pageKey , cancellationToken);
}