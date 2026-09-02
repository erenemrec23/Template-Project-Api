using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Persistance.Configurations.App
{
    public sealed class AppUserRoleConfiguration : IEntityTypeConfiguration<AppUserRole>
    {
        public void Configure(EntityTypeBuilder<AppUserRole> builder)
        {
            builder.ToTable("AppUserRoles");

            // Bileşik PK: bir (kullanıcı, rol) çifti tekildir. Id kolonu artık yok.
            builder.HasKey(x => new { x.AppUserId, x.AppRoleId });

            builder.HasOne(x => x.AppUser)
       .WithMany(x => x.AppUserRoles)
       .HasForeignKey(x => x.AppUserId)
       .OnDelete(DeleteBehavior.Cascade)
       .IsRequired(false);

            builder.HasOne(x => x.AppRole)
                   .WithMany()
                   .HasForeignKey(x => x.AppRoleId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired(false);
        }
    }
}