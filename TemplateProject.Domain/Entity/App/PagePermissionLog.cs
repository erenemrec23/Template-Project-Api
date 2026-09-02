using QrAssignment.Domain.Abstractions;
using QrAssignment.Domain.Shared.PagePermission;

namespace QrAssignment.Domain.Entity.App
{
    // Salt-ekleme (append-only) yetki değişiklik günlüğü. Asla update/delete edilmez.
    public sealed class PagePermissionLog : BaseEntity, IMustHaveTenant
    {
        // Sahip: kimin yetkisi değişti (kullanıcı ya da rol)
        public PermissionOwnerType OwnerType { get; set; }
        public Guid OwnerId { get; set; }

        // Hedef: sayfa mı grup mu
        public PermissionTargetType TargetType { get; set; }
        public int? PageId { get; set; }
        public short? MenuGroupId { get; set; }

        public PermissionChangeAction Action { get; set; }

        // Neydi → ne oldu
        public PageAccessFlags? OldValue { get; set; }
        public PageAccessFlags? NewValue { get; set; }

        // Hangi ekrandan yapıldı (ör. "user-form", "role-form", "permission-wizard")
        public string? SourcePage { get; set; }

        public Guid? TenantId { get; set; }

        // "Kim" = CreatedByUserId, "Ne zaman" = CreatedDate (BaseEntity'den, interceptor damgalar)
    }
}