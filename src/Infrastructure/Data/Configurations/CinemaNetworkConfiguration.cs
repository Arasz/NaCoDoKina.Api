using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Entities.Cinemas;

namespace NaCoDoKina.Api.Data.Configurations
{
    public class CinemaNetworkConfiguration : IEntityTypeConfiguration<CinemaNetwork>
    {
        public void Configure(EntityTypeBuilder<CinemaNetwork> builder)
        {
            builder.Property(network => network.Name)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}