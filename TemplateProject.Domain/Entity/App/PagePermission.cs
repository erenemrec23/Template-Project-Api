using QrAssignment.Domain.Abstractions;
using QrAssignment.Domain.Shared.PagePermission;

namespace QrAssignment.Domain.Entity.App
{
    public sealed class PagePermission : BaseEntity, IMustHaveTenant
    { 

        // Sahip: tam biri dolu (CHECK)
        public Guid? UserId { get; set; }
        public AppUser? User { get; set; }
        public Guid? RoleId { get; set; }
        public AppRole? Role { get; set; }

        // Hedef: tam biri dolu (CHECK) — tek sayfa YA DA tüm bir menü grubu
        public int? PageId { get; set; }              // artık nullable
        public Page? Page { get; set; }
        public short? MenuGroupId { get; set; }       // yeni — MenuGroup.Id (short)
        public MenuGroup? MenuGroup { get; set; }

        public PageAccessFlags PermissionValue { get; set; }
        public Guid? TenantId { get; set; }

        // Sayfa hedefli (mevcut isimler korundu → repolar bozulmaz)
        public static PagePermission ForUser(Guid userId, int pageId, PageAccessFlags value, Guid? tenantId)
            => new() { UserId = userId, PageId = pageId, PermissionValue = value, TenantId = tenantId };

        public static PagePermission ForRole(Guid roleId, int pageId, PageAccessFlags value, Guid? tenantId)
            => new() { RoleId = roleId, PageId = pageId, PermissionValue = value, TenantId = tenantId };

        // Grup hedefli (yeni)
        public static PagePermission ForUserGroup(Guid userId, short menuGroupId, PageAccessFlags value, Guid? tenantId)
            => new() { UserId = userId, MenuGroupId = menuGroupId, PermissionValue = value, TenantId = tenantId };

        public static PagePermission ForRoleGroup(Guid roleId, short menuGroupId, PageAccessFlags value, Guid? tenantId)
            => new() { RoleId = roleId, MenuGroupId = menuGroupId, PermissionValue = value, TenantId = tenantId };
    }
}