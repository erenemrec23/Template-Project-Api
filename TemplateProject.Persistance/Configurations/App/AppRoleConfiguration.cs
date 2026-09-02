using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Persistance.Configurations.App
{
    public sealed class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.ToTable("AppRoles");
             
            builder.Property<byte[]>("RowVersion")
                   .IsRowVersion();


            builder.HasOne(x => x.CreatedByUser)
                   .WithMany()
                   .HasForeignKey(x => x.CreatedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ModifiedByUser)
                   .WithMany()
                   .HasForeignKey(x => x.ModifiedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}