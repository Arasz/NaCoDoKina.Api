using ApplicationCore.Entities.Cinemas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
    public class CinemaNetworkConfiguration : IEntityTypeConfiguration<CinemaNetwork>
    {
        public void Configure(EntityTypeBuilder<CinemaNetwork> builder)
        {
            builder
                .HasIndex(network => network.Name)
                .IsUnique();

            builder
                .Property(network => network.Name)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}