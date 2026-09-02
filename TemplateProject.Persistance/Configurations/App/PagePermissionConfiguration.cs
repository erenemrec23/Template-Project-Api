using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Persistance.Configurations.App
{
    public sealed class PagePermissionConfiguration : IEntityTypeConfiguration<PagePermission>
    {
        public void Configure(EntityTypeBuilder<PagePermission> builder)
        {
            builder.ToTable("PagePermissions", t =>
                    {
                        t.HasCheckConstraint("CK_PagePermission_SingleOwner",
                            "([UserId] IS NOT NULL AND [RoleId] IS NULL) OR ([UserId] IS NULL AND [RoleId] IS NOT NULL)");

                        // YENİ: hedef de tam biri olmalı — ya Page ya MenuGroup
                        t.HasCheckConstraint("CK_PagePermission_SingleTarget",
                            "([PageId] IS NOT NULL AND [MenuGroupId] IS NULL) OR ([PageId] IS NULL AND [MenuGroupId] IS NOT NULL)");
                    });

            builder.HasKey(x => x.Id);
            builder.Property(x => x.PermissionValue).HasConversion<int>();

            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Page).WithMany().HasForeignKey(x => x.PageId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.MenuGroup).WithMany().HasForeignKey(x => x.MenuGroupId).OnDelete(DeleteBehavior.Restrict);

            // Aynı sahip + aynı hedef tek satır. PageId artık nullable olduğu için
            // filtrelere "hedef NOT NULL" eklemek ŞART (yoksa grup satırları page-index'i bozar).
            builder.HasIndex(x => new { x.UserId, x.PageId }).IsUnique()
                       .HasFilter("[UserId] IS NOT NULL AND [PageId] IS NOT NULL");
            builder.HasIndex(x => new { x.RoleId, x.PageId }).IsUnique()
                       .HasFilter("[RoleId] IS NOT NULL AND [PageId] IS NOT NULL");
            builder.HasIndex(x => new { x.UserId, x.MenuGroupId }).IsUnique()
                       .HasFilter("[UserId] IS NOT NULL AND [MenuGroupId] IS NOT NULL");
            builder.HasIndex(x => new { x.RoleId, x.MenuGroupId }).IsUnique()
                       .HasFilter("[RoleId] IS NOT NULL AND [MenuGroupId] IS NOT NULL");
        }
    }
}
