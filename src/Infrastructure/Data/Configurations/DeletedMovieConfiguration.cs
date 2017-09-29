using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NaCoDoKina.Api.Entities.Movies;

namespace NaCoDoKina.Api.Data.Configurations
{
    public class DeletedMovieConfiguration : IEntityTypeConfiguration<DeletedMovies>
    {
        public void Configure(EntityTypeBuilder<DeletedMovies> builder)
        {
            builder
                .HasKey(mark => new { mark.MovieId, mark.UserId });
        }
    }
}