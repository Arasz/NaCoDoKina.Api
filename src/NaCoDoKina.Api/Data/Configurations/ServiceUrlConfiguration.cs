using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NaCoDoKina.Api.Entities;

namespace NaCoDoKina.Api.Data.Configurations
{
    public class ServiceUrlConfiguration : IEntityTypeConfiguration<ServiceUrl>
    {
        public void Configure(EntityTypeBuilder<ServiceUrl> builder)
        {
            builder
                .HasIndex(url => url.Name)
                .IsUnique();

            builder
                .Property(url => url.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder
                .Property(url => url.Url)
                .IsRequired();
        }
    }
}