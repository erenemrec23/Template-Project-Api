//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using QrAssignment.Domain.Entity.App;

//namespace QrAssignment.Persistance.Configurations.App
//{
//    public sealed class AppRoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<Guid>>
//    {
//        public void Configure(EntityTypeBuilder<IdentityRoleClaim<Guid>> builder)
//        {
//            builder.ToTable("AppRoleClaims");

//            builder.Property<byte[]>("RowVersion")
//                   .IsRowVersion();

//            builder.HasOne<AppRole>()
//                   .WithMany(u => u.Claims)
//                   .HasForeignKey(uc => uc.RoleId)
//                   .IsRequired()
//                   .OnDelete(DeleteBehavior.Cascade);
//        }
//    }
//}