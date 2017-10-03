using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NaCoDoKina.Api.Entities.Resources;

namespace NaCoDoKina.Api.Data.Configurations
{
    public class ResourceLinkConfiguration : IEntityTypeConfiguration<MediaLink>, IEntityTypeConfiguration<ReviewLink>
    {
        public void Configure(EntityTypeBuilder<MediaLink> builder)
        {
            builder
                .HasIndex(link => link.Url)
                .IsUnique();

            builder
                .Property(link => link.Url)
                .HasMaxLength(300)
                .IsRequired();

            builder
                .Property(link => link.MediaType)
                .IsRequired();
        }

        public void Configure(EntityTypeBuilder<ReviewLink> builder)
        {
            builder
                .HasIndex(link => link.Name);

            builder
                .HasIndex(link => link.Url)
                .IsUnique();

            builder
                .Property(link => link.Url)
                .HasMaxLength(300)
                .IsRequired();

            builder
                .Property(link => link.LogoUrl)
                .HasMaxLength(300);

            builder
                .Property(link => link.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(link => link.Rating)
                .IsRequired()
                .HasDefaultValue(0);
        }
    }
}