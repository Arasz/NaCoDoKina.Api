using Microsoft.EntityFrameworkCore;
using NaCoDoKina.Api.Data.Configurations;
using NaCoDoKina.Api.Entities.Cinemas;
using NaCoDoKina.Api.Entities.Movies;
using NaCoDoKina.Api.Entities.Resources;

namespace NaCoDoKina.Api.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public DbSet<Cinema> Cinemas { get; set; }

        public DbSet<CinemaNetwork> CinemaNetworks { get; set; }

        public DbSet<MovieShowtime> MovieShowtimes { get; set; }

        public DbSet<DisabledMovie> DisabledMovies { get; set; }

        public DbSet<MovieDetails> MovieDetails { get; set; }

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
            modelBuilder.ApplyConfiguration<ReviewLink>(new ResourceLinkConfiguration());
            modelBuilder.ApplyConfiguration<MediaLink>(new ResourceLinkConfiguration());
            modelBuilder.ApplyConfiguration(new DeletedMovieConfiguration());
        }
    }
}