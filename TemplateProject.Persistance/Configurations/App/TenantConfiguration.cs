using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Configurations.Base;

namespace QrAssignment.Persistance.Configurations.App
{
    public sealed class TenantConfiguration : BaseEntityConfiguration<Tenant>
    {
        public override void Configure(EntityTypeBuilder<Tenant> builder)
        {
            base.Configure(builder);

            builder.ToTable("Tenants");

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasIndex(x => x.Name)
                   .IsUnique();
        }
    }
}