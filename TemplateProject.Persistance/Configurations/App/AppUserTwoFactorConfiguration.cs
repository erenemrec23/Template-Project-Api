using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Persistance.Configurations.App
{
    public sealed class AppUserTwoFactorConfiguration : IEntityTypeConfiguration<AppUserTwoFactor>
    {
        public void Configure(EntityTypeBuilder<AppUserTwoFactor> builder)
        {
            builder.ToTable("AppUserTwoFactor");

            // Shared primary key -> gercek 1-1
            builder.HasKey(x => x.AppUserId);

            builder.Property(x => x.SecretKey).HasMaxLength(256);

            builder.HasOne(x => x.AppUser)
                   .WithOne(x => x.TwoFactor)
                   .HasForeignKey<AppUserTwoFactor>(x => x.AppUserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}