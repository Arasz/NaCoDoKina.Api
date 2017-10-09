using ApplicationCore.Entities.Movies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
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