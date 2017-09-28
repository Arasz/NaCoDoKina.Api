using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NaCoDoKina.Api.Entities.Movies;

namespace NaCoDoKina.Api.Data.Configurations
{
    public class MovieShowtimeConfiguration : IEntityTypeConfiguration<MovieShowtime>
    {
        public void Configure(EntityTypeBuilder<MovieShowtime> builder)
        {
            builder
                .Property(showtime => showtime.ShowTime)
                .IsRequired();

            builder
                .Property(showtime => showtime.Language)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(showtime => showtime.ShowType)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .HasOne(showtime => showtime.Movie)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(showtime => showtime.Cinema)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}