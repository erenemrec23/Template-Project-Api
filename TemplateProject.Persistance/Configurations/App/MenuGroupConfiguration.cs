using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QrAssignment.Domain.Entity.App;

namespace QrAssignment.Persistance.Configurations.App
{
    public sealed class MenuGroupConfiguration : IEntityTypeConfiguration<MenuGroup>
    {
        public void Configure(EntityTypeBuilder<MenuGroup> builder)
        {
            builder.ToTable("MenuGroups");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Key).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Icon).HasMaxLength(100);

            builder.HasMany(x => x.Pages)
                   .WithOne(x => x.MenuGroup)
                   .HasForeignKey(x => x.MenuGroupId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

    public sealed class PageConfiguration : IEntityTypeConfiguration<Page>
    {
        public void Configure(EntityTypeBuilder<Page> builder)
        {
            builder.ToTable("Pages");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.PageKey).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Key).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Icon).HasMaxLength(100);
            builder.Property(x => x.Route).HasMaxLength(250);
            builder.Property(x => x.ShowInMenu).HasDefaultValue(true);

            builder.HasIndex(x => x.PageKey).IsUnique();
        }
    }
}