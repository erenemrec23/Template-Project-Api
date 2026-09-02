using Audit.Core;
using Audit.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Repositories;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Entity.Audit;
using QrAssignment.Persistance.Context;
using QrAssignment.Persistance.Exceptions;
using QrAssignment.Persistance.Interceptors;
using QrAssignment.Persistance.Options;
using QrAssignment.Persistance.Repositories;
using QrAssignment.Persistance.Services;
using System.Security.Claims;
using System.Text.Json;

namespace QrAssignment.Persistance
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityCore<AppUser>()
        .AddRoles<AppRole>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders(); 

            // EF store'larını AutoSaveChanges=false olanlarla değiştir (sonra gelmeli ki kazansın)
            services.AddScoped<IRoleStore<AppRole>, AppRoleStore>();
            services.AddScoped<IUserStore<AppUser>, AppUserStore>();
            services.AddScoped<AuditInterceptor>();

            services.AddDbContext<AppDbContext>((sp, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

                options.AddInterceptors(sp.GetRequiredService<AuditInterceptor>());
            });

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPermissionSyncService, PermissionSyncService>();
            
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<IDbExceptionTranslator, SqlServerExceptionTranslator>();

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;

                options.User.RequireUniqueEmail = true;
            })
    .AddEntityFrameworkStores<AppDbContext>();

            //services.AddScoped<IQrLocationRepository, QrLocationRepository>();
            services.Scan(scan => scan
                .FromCallingAssembly()
                .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Repository")))
                .AsImplementedInterfaces()
                .WithScopedLifetime()); 
            Audit.Core.Configuration.Setup()
                .UseEntityFramework(ef => ef
                    .AuditTypeMapper(t => typeof(SystemAuditLog))
                    .AuditEntityAction<SystemAuditLog>((ev, entry, auditEntity) =>
                    {
                        // 1. O ANKİ İŞLEMİ YAPAN DB CONTEXT'İ YAKALA
                        var dbContext = ev.GetEntityFrameworkEvent().GetDbContext();
                        var httpContextAccessor = dbContext.GetService<IHttpContextAccessor>();

                        var userId = httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                        var tenantClaim = httpContextAccessor?.HttpContext?.User?.FindFirst("TenantId")?.Value;
                        auditEntity.UserId = userId;
                        if (Guid.TryParse(tenantClaim, out var tenantId))
                        {
                            auditEntity.TenantId = tenantId;
                        }
                        auditEntity.TableName = entry.Table;
                        auditEntity.Action = entry.Action;
                        auditEntity.PrimaryKey = JsonSerializer.Serialize(entry.PrimaryKey);

                        auditEntity.ColumnValues = JsonSerializer.Serialize(entry.ColumnValues);
                        if (entry.Action == "Insert")
                        {
                            auditEntity.OldValues = null;
                            auditEntity.NewValues = JsonSerializer.Serialize(entry.ColumnValues);
                        }
                        else if (entry.Action == "Update")
                        {
                            auditEntity.OldValues = entry.Changes == null ? null :
                                JsonSerializer.Serialize(entry.Changes.ToDictionary(c => c.ColumnName, c => c.OriginalValue));

                            auditEntity.NewValues = entry.Changes == null ? null :
                                JsonSerializer.Serialize(entry.Changes.ToDictionary(c => c.ColumnName, c => c.NewValue));
                        }
                        else if (entry.Action == "Delete")
                        {
                            auditEntity.OldValues = JsonSerializer.Serialize(entry.ColumnValues);
                            auditEntity.NewValues = null;
                        }
                    })
        .IgnoreMatchedProperties(true)
    ); 
            services.Configure<TwoFactorOptions>(configuration.GetSection(TwoFactorOptions.SectionName));
            services.AddScoped<ITwoFactorService, TwoFactorService>(); // zaten eklediysen tekrar ekleme
            return services;
        }

    }
}
