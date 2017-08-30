using Microsoft.EntityFrameworkCore;
using NaCoDoKina.Api.Data.Configurations;
using NaCoDoKina.Api.Entities;

namespace NaCoDoKina.Api.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Movie>(new MovieConfiguration());
            modelBuilder.ApplyConfiguration<MovieDetails>(new MovieConfiguration());
            modelBuilder.ApplyConfiguration(new CinemaConfiguration());
            modelBuilder.ApplyConfiguration(new CinemaNetworkConfiguration());
            modelBuilder.ApplyConfiguration(new MovieShowtimeConfiguration());
        }
    }
}