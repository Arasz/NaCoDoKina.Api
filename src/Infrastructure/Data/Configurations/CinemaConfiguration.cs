using ApplicationCore.Entities.Cinemas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class CinemaConfiguration : IEntityTypeConfiguration<Cinema>
    {
        public void Configure(EntityTypeBuilder<Cinema> builder)
        {
            builder
                .HasIndex(cinema => cinema.Name)
                .IsUnique();

            builder.Property(cinema => cinema.Name)
                .IsRequired()
                .HasMaxLength(80);

            builder.Property(cinema => cinema.Address)
                .IsRequired()
                .HasMaxLength(80);

            builder.HasIndex(cinema => cinema.ExternalId);

            builder.OwnsOne(cinema => cinema.Location);
        }
    }
}