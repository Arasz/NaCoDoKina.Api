using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.Entities;
using Ploeh.AutoFixture;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.IntegrationTests.Api.DatabaseInitializer
{
    public class MovieDatabaseTestDataInitialization : IDbInitialize<ApplicationContext>
    {
        private readonly ILogger<MovieDatabaseTestDataInitialization> _logger;
        public ApplicationContext DbContext { get; }

        private readonly Fixture _fixture;

        public MovieDatabaseTestDataInitialization(ApplicationContext applicationContext, ILogger<MovieDatabaseTestDataInitialization> logger, Fixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            DbContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));

            CustomizeFixture();
        }

        public async Task InitializeAsync()
        {
            if (DbContext.Database.EnsureDeleted())
                _logger.LogInformation("Database was deleted");

            _logger.LogInformation("Starting database migrations");

            try
            {
                await DbContext.Database.MigrateAsync();
            }
            catch (Exception migrationException)
            {
                _logger.LogInformation("Database migrations failed with error {@migrationException}", migrationException);
                throw;
            }
            _logger.LogInformation("Database migrations completed");

            var movies = _fixture.CreateMany<Movie>(15)
                .ToArray();
            var cinemaNetworks = _fixture.CreateMany<CinemaNetwork>(2)
                .ToArray();
            var cinemas = cinemaNetworks
                .SelectMany(network => network.Cinemas)
                .ToArray();
            var movieShowtimes = _fixture.CreateMany<MovieShowtime>(15)
                .ToArray();

            var rnd = new Random(5);
            for (var i = 0; i < movieShowtimes.Length; i++)
            {
                movieShowtimes[i].Cinema = cinemas[rnd.Next(0, cinemas.Length)];
                movieShowtimes[i].Movie = movies[i];
            }

            DbContext.CinemaNetworks.AddRange(cinemaNetworks);
            DbContext.MovieShowtimes.AddRange(movieShowtimes);
            await DbContext.SaveChangesAsync();
        }

        private void CustomizeFixture()
        {
            _fixture.Customize<Movie>(composer => composer
                .Without(movie => movie.Id));

            _fixture.Customize<MovieDetails>(composer => composer
                .Without(details => details.Id)
                .Without(details => details.MovieId));
            _fixture.Customize<Cinema>(composer => composer
                .Without(cinema => cinema.Id));

            _fixture.Customize<CinemaNetwork>(composer => composer
                .Without(cinemaNetwork => cinemaNetwork.Id));

            _fixture.Customize<Location>(composer => composer
                .With(location => location.Longitude, 52.44056)
                .With(location => location.Latitude, 16.919235));

            _fixture.Customize<MovieShowtime>(composer => composer
                .Without(showtime => showtime.Id)
                .Without(showtime => showtime.Cinema)
                .Without(showtime => showtime.Movie));
        }
    }
}