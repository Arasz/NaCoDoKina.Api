using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NaCoDoKina.Api.Entities;

namespace NaCoDoKina.Api.Data.Configurations
{
    internal class CinemaNetworkConfiguration : IEntityTypeConfiguration<CinemaNetwork>
    {
        public void Configure(EntityTypeBuilder<CinemaNetwork> builder)
        {
            builder.Property(network => network.Name)
                .IsRequired()
                .HasMaxLength(255);
        }
    }

    public class CinemaConfiguration : IEntityTypeConfiguration<Cinema>
    {
        public void Configure(EntityTypeBuilder<Cinema> builder)
        {
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