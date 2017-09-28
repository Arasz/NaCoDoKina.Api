using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Entities.Cinemas;

namespace NaCoDoKina.Api.Data.Configurations
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
                .HasMaxLength(225);

            builder.Property(cinema => cinema.Address)
                .IsRequired()
                .HasMaxLength(255);

            builder.OwnsOne(cinema => cinema.Location);
        }
    }
}