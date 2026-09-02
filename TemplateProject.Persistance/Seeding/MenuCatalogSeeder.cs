using Microsoft.EntityFrameworkCore;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Context;

namespace QrAssignment.Persistance.Seeding
{
    public sealed class MenuCatalogSeeder
    {
        private readonly AppDbContext _context; // ← senin DbContext tipinle değiştir

        public MenuCatalogSeeder(AppDbContext context) => _context = context;

        public async Task SeedAsync(CancellationToken ct = default)
        {
            foreach (var g in AppMenuCatalog.BuildGroups())
            {
                var existing = await _context.Set<MenuGroup>().FindAsync([g.Id], ct);
                if (existing is null) _context.Add(g);
                else { existing.Key = g.Key; existing.Icon = g.Icon; existing.Order = g.Order; }
            }

            foreach (var p in AppMenuCatalog.BuildPages())
            {
                var existing = await _context.Set<Page>().FindAsync([p.Id], ct);
                if (existing is null) _context.Add(p);
                else
                {
                    existing.PageKey = p.PageKey;
                    existing.Key = p.Key;
                    existing.Icon = p.Icon;
                    existing.Route = p.Route;
                    existing.Order = p.Order;
                    existing.ShowInMenu = p.ShowInMenu;
                    existing.MenuGroupId = p.MenuGroupId;
                }
            }

            // Not: koddan SİLİNEN sayfaları burada otomatik silmiyorum — PagePermission FK'si
            // olabilir. Silme gerekirse ayrı, kontrollü bir adımda ele alırız.
            await _context.SaveChangesAsync(ct);
        }
    }
}