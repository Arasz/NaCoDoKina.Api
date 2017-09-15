using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NaCoDoKina.Api.Entities;

namespace NaCoDoKina.Api.Data.Configurations
{
    public class ServiceUrlConfiguration : IEntityTypeConfiguration<ServiceUrl>
    {
        public void Configure(EntityTypeBuilder<ServiceUrl> builder)
        {
            builder.HasKey(url => new { url.Name, url.Url });
        }
    }
}