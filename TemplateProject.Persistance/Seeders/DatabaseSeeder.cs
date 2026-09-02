using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QrAssignment.Application.Services; // ITenantIdService için
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared.PagePermission;
using QrAssignment.Persistance.Context;

namespace QrAssignment.Persistence.Seeders
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var tenantRepository = scope.ServiceProvider.GetRequiredService<ITenantRepository>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            // 1. ITenantIdService'i aynı scope içinden çekiyoruz
            var tenantIdService = scope.ServiceProvider.GetRequiredService<ITenantIdService>();

            // 2. Şemayı Oluştur (Migration)
            await context.Database.MigrateAsync();

            // 3. Transaction Başlat
            await using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                // A) TENANT OLUŞTURMA
                string defaultTenantName = "Default Tenant";
                var defaultTenant = await context.Set<Tenant>()
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(t => t.Name == defaultTenantName);

                if (defaultTenant == null)
                {
                    defaultTenant = new Tenant
                    { 
                        Name = defaultTenantName
                    };

                    // NOT: Tenant ilk eklenirken henüz TenantId belirsiz olabileceğinden
                    // geçici olarak oluşturduğumuz bu ID'yi önceden de atayabiliriz:
                    tenantIdService.SetTenantId(defaultTenant.Id);

                    await tenantRepository.AddAsync(defaultTenant, CancellationToken.None);
                    await context.SaveChangesAsync();
                }
                else
                {
                    // Tenant zaten varsa var olan ID'yi servise override olarak set ediyoruz
                    tenantIdService.SetTenantId(defaultTenant.Id);
                }

                // *** KRİTİK NOKTA ***
                // Artık tenantIdService.SetTenantId() çağrıldığı için,
                // bundan sonraki tüm context.SaveChangesAsync() ve AuditInterceptor işlemlerinde
                // GetTenantId() metodu exception fırlatmayacak, defaultTenant.Id değerini dönecektir.

                // B) ADMIN ROLÜ OLUŞTURMA
                string adminRoleName = "Admin";
                var adminRole = await context.Set<AppRole>()
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(r => r.Name == adminRoleName);

                if (adminRole == null)
                {
                    adminRole = new AppRole
                    {
                        Id = Guid.NewGuid(),
                        Name = adminRoleName,
                        NormalizedName = adminRoleName.ToUpperInvariant(),
                        TenantId = defaultTenant.Id,
                        CreatedDate = DateTimeOffset.UtcNow,
                        ConcurrencyStamp = Guid.NewGuid().ToString(),
                        IsPassived = false
                    };

                    await context.Set<AppRole>().AddAsync(adminRole);
                    await context.SaveChangesAsync();
                }

                // C) PAGE PERMISSION (TÜM SAYFALAR VE GRUPLAR İÇİN FULL YETKİ)
                var fullPermissionValue = PageAccessFlags.All | PageAccessFlags.ManagePagePermissions;

                // 1. Sayfalar (Pages)
                var allPages = await context.Set<Page>().IgnoreQueryFilters().ToListAsync();
                var existingPagePermissions = await context.Set<PagePermission>()
                    .IgnoreQueryFilters()
                    .Where(pp => pp.RoleId == adminRole.Id && pp.PageId != null)
                    .ToListAsync();

                foreach (var page in allPages)
                {
                    var existing = existingPagePermissions.FirstOrDefault(p => p.PageId == page.Id);
                    if (existing == null)
                    {
                        var pagePerm = PagePermission.ForRole(adminRole.Id, page.Id, fullPermissionValue, defaultTenant.Id);
                        await context.Set<PagePermission>().AddAsync(pagePerm);
                    }
                    else
                    {
                        existing.PermissionValue = fullPermissionValue;
                        context.Set<PagePermission>().Update(existing);
                    }
                }

                // 2. Menü Grupları (MenuGroups)
                var allMenuGroups = await context.Set<MenuGroup>().IgnoreQueryFilters().ToListAsync();
                var existingGroupPermissions = await context.Set<PagePermission>()
                    .IgnoreQueryFilters()
                    .Where(pp => pp.RoleId == adminRole.Id && pp.MenuGroupId != null)
                    .ToListAsync();

                foreach (var group in allMenuGroups)
                {
                    var existing = existingGroupPermissions.FirstOrDefault(g => g.MenuGroupId == group.Id);
                    if (existing == null)
                    {
                        var groupPerm = PagePermission.ForRoleGroup(adminRole.Id, group.Id, fullPermissionValue, defaultTenant.Id);
                        await context.Set<PagePermission>().AddAsync(groupPerm);
                    }
                    else
                    {
                        existing.PermissionValue = fullPermissionValue;
                        context.Set<PagePermission>().Update(existing);
                    }
                }

                await context.SaveChangesAsync();

                // D) ADMIN KULLANICISI OLUŞTURMA
                string adminEmail = "admin@qrassignment.com";
                string adminUsername = "admin";

                var adminUser = await context.Set<AppUser>()
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(u => u.Email == adminEmail);

                if (adminUser == null)
                {
                    adminUser = AppUser.Create("System", "Admin", adminUsername, adminEmail);
                    adminUser.Id = Guid.NewGuid();
                    adminUser.TenantId = defaultTenant.Id;
                    adminUser.EmailConfirmed = true;
                    adminUser.CreatedDate = DateTimeOffset.UtcNow;
                    adminUser.IsPassived = false;

                    var createUserResult = await userManager.CreateAsync(adminUser, "Admin123!*");
                    if (!createUserResult.Succeeded)
                    {
                        throw new Exception($"Admin kullanıcısı oluşturulamadı: {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
                    }

                    // E) APP USER ROLE İLİŞKİSİ
                    var userRoleExist = await context.Set<AppUserRole>()
                        .IgnoreQueryFilters()
                        .AnyAsync(ur => ur.AppUserId == adminUser.Id && ur.AppRoleId == adminRole.Id);

                    if (!userRoleExist)
                    {
                        var appUserRole = new AppUserRole
                        {
                            AppUserId = adminUser.Id,
                            AppRoleId = adminRole.Id
                        };

                        await context.Set<AppUserRole>().AddAsync(appUserRole);
                        await context.SaveChangesAsync();
                    }
                }

                // TÜM İŞLEMLER BAŞARILI İSE COMMIT ET
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Database Seeding işlemi sırasında bir hata oluştu ve tüm işlemler geri alındı. Detay: {ex.Message}", ex);
            }
        }
    }
}