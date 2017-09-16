using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NaCoDoKina.Api.Entities;

namespace NaCoDoKina.Api.Data.Configurations
{
    public class MovieShowtimeConfiguration : IEntityTypeConfiguration<MovieShowtime>
    {
        public void Configure(EntityTypeBuilder<MovieShowtime> builder)
        {
            builder.Property(showtime => showtime.ShowTime)
                .IsRequired();
        }
    }
}