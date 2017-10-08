using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NaCoDoKina.Api.Entities.Movies;

namespace NaCoDoKina.Api.Data.Configurations
{
    public class DeletedMovieConfiguration : IEntityTypeConfiguration<DisabledMovie>
    {
        public void Configure(EntityTypeBuilder<DisabledMovie> builder)
        {
            builder
                .HasKey(mark => new { mark.MovieId, mark.UserId });
        }
    }
}