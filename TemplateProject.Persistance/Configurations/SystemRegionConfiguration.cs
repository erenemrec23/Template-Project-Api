using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAssignment.Domain.Entity.System;
using QrAssignment.Persistance.Configurations.Base;

namespace QrAssignment.Persistance.Configurations
{
    // NOT: SystemRegion, BaseEntity'den türemiyorsa bu satırı eskisi gibi
    // "IEntityTypeConfiguration<SystemRegion>" olarak geri al ve base.Configure() çağrısını kaldır.
    public sealed class SystemRegionConfiguration : BaseEntityConfiguration<SystemRegion>
    {
        public override void Configure(EntityTypeBuilder<SystemRegion> builder)
        {
            base.Configure(builder);

            builder.ToTable("SystemRegions");

            builder.HasOne(c => c.ParentRegion)
                   .WithMany(b => b.SubLocations)
                   .HasForeignKey(c => c.ParentRegionId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.Name).HasMaxLength(250).IsRequired();
            builder.Property(p => p.Code).HasMaxLength(250);
            builder.Property(p => p.Level).HasConversion<string>().IsRequired();
        }
    }
}