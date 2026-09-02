using Audit.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Abstractions;
using QrAssignment.Domain.Entity;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Entity.Audit;
using QrAssignment.Domain.Entity.System;
using System.Linq.Expressions;
using System.Reflection;

namespace QrAssignment.Persistance.Context;

public class AppDbContext : AuditDbContext
{
    private readonly ITenantIdService _tenantService;

    public AppDbContext(DbContextOptions<AppDbContext> options, ITenantIdService tenantService) : base(options)
    {
        _tenantService = tenantService;
    }

    public DbSet<PagePermissionLog> PagePermissionLogs { get; set; }
    public DbSet<SystemAuditLog> SystemAuditLogs { get; set; }

    public DbSet<AppUser> AppUsers { get; set; }

    public DbSet<AppUserRole> AppUserRole { get; set; }

    public DbSet<AppRole> AppRoles { get; set; }
    public DbSet<PagePermission> PagePermissions { get; set; }
    public DbSet<Page> Pages { get; set; }
    public DbSet<Page> MenuGroups { get; set; }
    public DbSet<AppUserRefreshToken> AppUserRefreshTokens { get; set; }

    // NOT: QrApplicantConfiguration zaten mevcut ve ApplyConfigurationsFromAssembly
    // açıldığı için entity modele dahil olacak. Ama context üzerinden sorgulayabilmek
    // (context.QrApplicants gibi) için DbSet'i açman gerekiyor. Entity hazır olduğunda
    // aşağıdaki satırı aç:
    // public DbSet<QrApplicant> QrApplicants { get; set; }

    public DbSet<QrLocation> QrLocations { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    
    public DbSet<SystemRegion> SystemRegions { get; set; }

    public DbSet<Tenant> Tenants { get; set; }

    public DbSet<AppUserTwoFactor> AppUserTwoFactors => Set<AppUserTwoFactor>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<IdentityUserClaim<Guid>>();
        modelBuilder.Ignore<IdentityRoleClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserLogin<string>>(); 
        modelBuilder.Ignore<IdentityUserToken<string>>();
        modelBuilder.Ignore<IdentityRole<string>>();
         
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var type = entityType.ClrType;

            // 1. Soft Delete Filtresi
            if (typeof(ISoftDelete).IsAssignableFrom(type))
            {
                // EF Core 10 Named Query Filter tanımı
                modelBuilder.Entity(type)
                    .HasQueryFilter("SoftDeleteFilter", ConvertFilterExpressionOfIsDeleted(type));
            }

            // 2. Tenant İzolasyonu Filtresi
            if (typeof(IMustHaveTenant).IsAssignableFrom(type))
            {
                // EF Core 10 Named Query Filter tanımı
                modelBuilder.Entity(type)
                    .HasQueryFilter("TenantFilter", CreateTenantExpression(type));
            }
        }
    }

    public Guid CurrentTenantId => _tenantService.GetTenantId();

    private static LambdaExpression ConvertFilterExpressionOfIsDeleted(Type entityType)
    {
        var parameter = Expression.Parameter(entityType, "p");
        var propertyAccess = Expression.Property(parameter, nameof(ISoftDelete.IsPassived));

        Expression falseConstant = Expression.Constant(false);

        if (propertyAccess.Type != typeof(bool))
        {
            falseConstant = Expression.Convert(falseConstant, propertyAccess.Type);
        }

        var equalExpression = Expression.Equal(propertyAccess, falseConstant);

        return Expression.Lambda(equalExpression, parameter);
    }

    // p => p.TenantId == _tenantProvider.TenantId ifadesini dinamik oluşturur
    private LambdaExpression CreateTenantExpression(Type type)
    {
        var parameter = Expression.Parameter(type, "p");

        // Entity üzerindeki TenantId property'sini alıyoruz (Guid ya da Guid?)
        var property = Expression.Property(parameter, nameof(IMustHaveTenant.TenantId));

        // DbContext üzerindeki CurrentTenantId değerini alıyoruz (Guid ya da int vb.)
        var tenantIdProperty = Expression.Property(Expression.Constant(this), nameof(CurrentTenantId));

        Expression compare;

        // Eğer entity'deki TenantId alanı Nullable (Guid?) ise ve DbContext'teki değer non-nullable (Guid) ise
        if (property.Type != tenantIdProperty.Type)
        {
            // DbContext'ten gelen non-nullable Guid değerini, Nullable<Guid>'e convert (cast) ediyoruz
            var convertedTenantId = Expression.Convert(tenantIdProperty, property.Type);
            compare = Expression.Equal(property, convertedTenantId);
        }
        else
        {
            // Tipler birebir aynı ise (örneğin ikisi de Guid ya da ikisi de Guid?) doğrudan karşılaştır
            compare = Expression.Equal(property, tenantIdProperty);
        }

        return Expression.Lambda(compare, parameter);
    }
}