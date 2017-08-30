using Microsoft.EntityFrameworkCore;
using NaCoDoKina.Api.Data.Configurations;

namespace NaCoDoKina.Api.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MovieConfiguration());
        }
    }
}