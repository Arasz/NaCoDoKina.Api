using Microsoft.EntityFrameworkCore;
using NaCoDoKina.Api.Data.Configurations;
using NaCoDoKina.Api.Entities;

namespace NaCoDoKina.Api.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Cinema> Cinemas { get; set; }

        public DbSet<CinemaNetwork> CinemaNetworks { get; set; }

        public DbSet<MovieShowtime> MovieShowtimes { get; set; }

        public DbSet<DeletedMovieMark> DeletedMovieMarks { get; set; }

        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration<Movie>(new MovieConfiguration());
            modelBuilder.ApplyConfiguration<MovieDetails>(new MovieConfiguration());
            modelBuilder.ApplyConfiguration(new CinemaConfiguration());
            modelBuilder.ApplyConfiguration(new CinemaNetworkConfiguration());
            modelBuilder.ApplyConfiguration(new MovieShowtimeConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceUrlConfiguration());
            modelBuilder.ApplyConfiguration(new DeletedMovieMarkConfiguration());
        }
    }
}