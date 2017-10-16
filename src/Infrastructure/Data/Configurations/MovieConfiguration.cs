using ApplicationCore.Entities.Movies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
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

            builder.HasIndex(movie => movie.Title);

            builder.Property(movie => movie.Title)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(80);

            builder
                .Property(movie => movie.PosterUrl)
                .HasMaxLength(200);

            builder
                .HasMany(movie => movie.ExternalMovies)
                .WithOne()
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable(TableName);
        }

        public void Configure(EntityTypeBuilder<MovieDetails> builder)
        {
            builder.ToTable(TableName);

            builder.Property(movie => movie.Title)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(80);

            builder.Property(movie => movie.OriginalTitle)
                .IsUnicode()
                .HasMaxLength(80);

            builder.Property(movie => movie.Description)
                .HasDefaultValue("No description")
                .IsUnicode();

            builder.Property(movie => movie.AgeLimit)
                .HasDefaultValue("Unspecified")
                .HasMaxLength(100);

            builder.Property(movie => movie.Genre)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(movie => movie.Director)
                .HasMaxLength(100);

            builder.Property(movie => movie.CrewDescription)
                .IsUnicode()
                .HasMaxLength(300);

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