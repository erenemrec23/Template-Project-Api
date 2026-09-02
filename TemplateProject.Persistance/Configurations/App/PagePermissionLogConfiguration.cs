using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Persistance.Configurations.App
{
    internal sealed class PagePermissionLogConfiguration : IEntityTypeConfiguration<PagePermissionLog>
    {
        public void Configure(EntityTypeBuilder<PagePermissionLog> b)
        {
            b.ToTable("PagePermissionLogs");
            b.Property(x => x.SourcePage).HasMaxLength(128);

            b.HasIndex(x => new { x.OwnerType, x.OwnerId });
            b.HasIndex(x => x.PageId);
            b.HasIndex(x => x.CreatedDate);
            // FK yok: PageId/MenuGroupId sadece referans; log kalıcı denetim izi
            b.Property(e => e.OwnerType).HasConversion<string>().HasMaxLength(20);
            b.Property(e => e.TargetType).HasConversion<string>().HasMaxLength(20);
            b.Property(e => e.Action).HasConversion<string>().HasMaxLength(20);
        }
    }
}