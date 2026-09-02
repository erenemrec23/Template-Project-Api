using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Persistance.Configurations.Base;

namespace QrAssignment.Persistance.Configurations.App
{
    // BaseEntityConfiguration'dan türetildi: AppUserRefreshToken, BaseEntity'den türüyor
    // (CreatedByUser/ModifiedByUser navigation'larına sahip). base.Configure() çağrısı
    // olmadan bu FK'lar EF tarafından "Unable to determine the relationship" hatasıyla
    // reddediliyordu, çünkü AppUser'a giden iki farklı yol (RefreshToken FK'sı +
    // Created/ModifiedByUser FK'ları) belirsizlik yaratıyor ve convention ile çözülemiyor.
    public sealed class AppUserRefreshTokenConfiguration : BaseEntityConfiguration<AppUserRefreshToken>
    {
        public override void Configure(EntityTypeBuilder<AppUserRefreshToken> builder)
        {
            base.Configure(builder);

            builder.ToTable("AppUserRefreshTokens"); 
            builder.HasOne(x => x.AppUser)
                   .WithOne(u => u.RefreshToken)
                   .HasForeignKey<AppUserRefreshToken>(x => x.AppUserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.RefreshToken)
                   .IsRequired()
                   .HasMaxLength(250);

            builder.Property(x => x.RefreshTokenExpires)
                   .IsRequired();
        }
    }
}