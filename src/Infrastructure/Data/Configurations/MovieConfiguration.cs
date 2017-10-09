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

            builder.Property(movie => movie.Title)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(80);

            builder
                .Property(movie => movie.PosterUrl)
                .HasMaxLength(200)
                .IsRequired();

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
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(80);

            builder.Property(movie => movie.Description)
                .IsRequired()
                .IsUnicode();

            builder.Property(movie => movie.AgeLimit)
                .HasDefaultValue()
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(movie => movie.Language)
                .HasDefaultValue(string.Empty)
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