using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NaCoDoKina.Api.Entities.Movies;

namespace NaCoDoKina.Api.Data.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie>, IEntityTypeConfiguration<MovieDetails>
    {
        private const string TableName = "Movies";

        public void Configure(EntityTypeBuilder<Movie> builder)
        {
            builder
                .HasOne(movie => movie.Details)
                .WithOne()
                .HasForeignKey<MovieDetails>(details => details.Id);

            builder.Property(movie => movie.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.ToTable(TableName);
        }

        public void Configure(EntityTypeBuilder<MovieDetails> builder)
        {
            builder.ToTable(TableName);

            builder
                .HasMany(movieDetail => movieDetail.MediaResources)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(movieDetail => movieDetail.MovieReviews)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}