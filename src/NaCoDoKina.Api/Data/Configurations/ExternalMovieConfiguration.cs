using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NaCoDoKina.Api.Entities.Movies;

namespace NaCoDoKina.Api.Data.Configurations
{
    public class ExternalMovieConfiguration : IEntityTypeConfiguration<ExternalMovie>
    {
        public void Configure(EntityTypeBuilder<ExternalMovie> builder)
        {
            builder
                .HasIndex(id => id.ExternalId)
                .IsUnique();

            builder
                .Property(movie => movie.MovieUrl)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}