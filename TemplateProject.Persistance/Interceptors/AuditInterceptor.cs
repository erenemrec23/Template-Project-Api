using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Abstractions;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared.PagePermission;

namespace QrAssignment.Persistance.Interceptors;

public sealed class AuditInterceptor : SaveChangesInterceptor
{
    private readonly ITenantIdService _tenantService;
    private readonly IUserContext _userContext;

    private readonly IPermissionChangeContext _permissionChangeContext;

    public AuditInterceptor(IUserContext userContext, ITenantIdService tenantService,
                            IPermissionChangeContext permissionChangeContext)
    {
        _userContext = userContext;
        _tenantService = tenantService;
        _permissionChangeContext = permissionChangeContext;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var dbContext = eventData.Context;
        if (dbContext is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        var currentUserId = _userContext.GetCurrentUserId();
        var currentTime = DateTimeOffset.UtcNow;



        // >>> YENİ: yetki değişikliklerini logla (soft-delete dönüşümünden ÖNCE) <
        var permissionEntries = dbContext.ChangeTracker.Entries<PagePermission>().ToList();
        if (permissionEntries.Count > 0)
        {
            var sourcePage = _permissionChangeContext.SourcePage;
            foreach (var pe in permissionEntries)
            {
                var log = BuildPermissionLog(pe, sourcePage);
                if (log is not null) dbContext.Add(log); // Added → aşağıdaki döngüler damgalar
            }
        }

        var entries = dbContext.ChangeTracker.Entries<IBaseEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.IsPassived = false;
                entry.Entity.CreatedByUserId = currentUserId;
                entry.Entity.CreatedDate = currentTime;

            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedByUserId = currentUserId;
                entry.Entity.ModifiedDate = currentTime;
                entry.Property(x => x.RevNum).IsModified = false;
            }
            else if (entry.State == EntityState.Deleted && entry.Entity.IsPassived == false)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsPassived = true;
                entry.Entity.ModifiedByUserId = currentUserId;
                entry.Entity.ModifiedDate = currentTime;
                entry.Property(x => x.RevNum).IsModified = false;   // ekleyin 
            }

        }



        var entriesHasTenantBaseId = dbContext.ChangeTracker.Entries<IMustHaveTenant>();

        foreach (var entry in entriesHasTenantBaseId)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.TenantId = _tenantService.GetTenantId();
            }

        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }


    private static PagePermissionLog? BuildPermissionLog(
        EntityEntry<PagePermission> entry, string? sourcePage)
    {
        var e = entry.Entity;

        // Sahip çöz
        PermissionOwnerType ownerType;
        Guid ownerId;
        if (e.UserId is Guid uid) { ownerType = PermissionOwnerType.User; ownerId = uid; }
        else if (e.RoleId is Guid rid) { ownerType = PermissionOwnerType.Role; ownerId = rid; }
        else return null; // sahipsiz satır — atla

        var targetType = e.PageId is not null
            ? PermissionTargetType.Page
            : PermissionTargetType.MenuGroup;

        PageAccessFlags? oldValue = null, newValue = null;
        PermissionChangeAction action;

        switch (entry.State)
        {
            case EntityState.Added:
                action = PermissionChangeAction.Added;
                newValue = e.PermissionValue;
                break;

            case EntityState.Deleted:
                action = PermissionChangeAction.Removed;
                oldValue = entry.OriginalValues.GetValue<PageAccessFlags>(nameof(PagePermission.PermissionValue));
                break;

            case EntityState.Modified:
                {
                    var wasPassived = entry.OriginalValues.GetValue<bool>(nameof(PagePermission.IsPassived));
                    var origValue = entry.OriginalValues.GetValue<PageAccessFlags>(nameof(PagePermission.PermissionValue));

                    if (!wasPassived && e.IsPassived)                 // aktif → pasif = kaldırma
                    {
                        action = PermissionChangeAction.Removed;
                        oldValue = origValue;
                    }
                    else if (wasPassived && !e.IsPassived)            // pasif → aktif = yeniden ekleme
                    {
                        action = PermissionChangeAction.Added;
                        newValue = e.PermissionValue;
                    }
                    else if (origValue != e.PermissionValue)          // değer değişti
                    {
                        action = PermissionChangeAction.Updated;
                        oldValue = origValue;
                        newValue = e.PermissionValue;
                    }
                    else return null;                                 // anlamlı yetki değişikliği yok
                    break;
                }

            default:
                return null;
        }

        return new PagePermissionLog
        {
            OwnerType = ownerType,
            OwnerId = ownerId,
            TargetType = targetType,
            PageId = e.PageId,
            MenuGroupId = e.MenuGroupId,
            Action = action,
            OldValue = oldValue,
            NewValue = newValue,
            SourcePage = sourcePage
            // Created* / TenantId → aşağıdaki mevcut döngüler damgalayacak
        };
    }
}