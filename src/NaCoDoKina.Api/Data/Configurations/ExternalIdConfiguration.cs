using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NaCoDoKina.Api.Entities.Movies;

namespace NaCoDoKina.Api.Data.Configurations
{
    public class ExternalIdConfiguration : IEntityTypeConfiguration<ExternalId>
    {
        public void Configure(EntityTypeBuilder<ExternalId> builder)
        {
            builder
                .HasIndex(id => id.MovieExternalId)
                .IsUnique();
        }
    }
}