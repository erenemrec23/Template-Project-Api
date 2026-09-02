using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAssignment.Domain.Entity;
using QrAssignment.Persistance.Configurations.Base;

namespace QrAssignment.Persistance.Configurations
{
    // NOT: QrApplicant, BaseEntity'den türemiyorsa bu satırı eskisi gibi
    // "IEntityTypeConfiguration<QrApplicant>" olarak geri al ve base.Configure() çağrısını kaldır.
    public sealed class QrApplicantConfiguration : BaseEntityConfiguration<QrApplicant>
    {
        public override void Configure(EntityTypeBuilder<QrApplicant> builder)
        {
            base.Configure(builder);

            builder.ToTable("QrApplicants");

            builder.Property(p => p.FirstName).HasMaxLength(250).IsRequired();
            builder.Property(p => p.LastName).HasMaxLength(250).IsRequired();
            builder.Property(p => p.Mail).HasMaxLength(250).IsRequired();
              
            builder.HasOne(x => x.Region)
                   .WithMany()
                   .HasForeignKey(x => x.RegionId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}