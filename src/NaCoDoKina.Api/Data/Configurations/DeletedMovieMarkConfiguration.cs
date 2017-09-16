using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NaCoDoKina.Api.Entities;

namespace NaCoDoKina.Api.Data.Configurations
{
    public class DeletedMovieMarkConfiguration : IEntityTypeConfiguration<DeletedMovieMark>
    {
        public void Configure(EntityTypeBuilder<DeletedMovieMark> builder)
        {
            builder
                .HasKey(mark => new { mark.MovieId, mark.UserId });
        }
    }
}