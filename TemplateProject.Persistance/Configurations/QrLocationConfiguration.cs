using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAssignment.Domain.Entity;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Configurations.Base;

namespace QrAssignment.Persistance.Configurations.App
{
    // NOT: QrLocation TenantBaseEntity'den türüyor. Eğer projenizde tenant kapsamlı
    // entity'ler için ayrı bir TenantBaseEntityConfiguration<T> varsa, base olarak onu
    // kullanın; TenantId FK / index yapılandırması orada merkezi tutulur.
    public sealed class QrLocationConfiguration : BaseEntityConfiguration<QrLocation>
    {
        public override void Configure(EntityTypeBuilder<QrLocation> builder)
        {
            base.Configure(builder);

            builder.ToTable("QrLocations");

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(x => x.LocationName)
                   .HasMaxLength(200);

            builder.HasIndex(x => x.Name)
                   .IsUnique();
        }
    }
}
