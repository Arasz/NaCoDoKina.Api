using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.Entities.Cinemas;
using NaCoDoKina.Api.Entities.Movies;
using Ploeh.AutoFixture;
using System;
using System.Linq;

namespace NaCoDoKina.Api.IntegrationTests.Api.DatabaseSeed
{
    public class ApplicationDataSeed : IDatabaseSeed
    {
        private readonly ILogger<ApplicationDataSeed> _logger;
        public ApplicationContext DbContext { get; }

        private readonly IFixture _fixture;

        public ApplicationDataSeed(ApplicationContext applicationContext, ILogger<ApplicationDataSeed> logger, IFixture fixture)
        {
            _fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            DbContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));

            CustomizeFixture();
        }

        public void Seed()
        {
            using (_logger.BeginScope(nameof(ApplicationDataSeed)))
            {
                _logger.LogInformation("Starting database migrations");

                try
                {
                    DbContext.Database.Migrate();
                }
                catch (Exception migrationException)
                {
                    _logger.LogInformation("Database migrations failed with error {@migrationException}", migrationException);
                    throw;
                }
                _logger.LogInformation("Database migrations completed");

                _logger.LogInformation("Generating and saving test data");

                GenerateAndSaveData();

                _logger.LogInformation("Initialization complete");
            }
        }

        private void GenerateAndSaveData()
        {
            var movies = _fixture.CreateMany<Movie>(30)
                .ToArray();

            var cinemas = _fixture.CreateMany<Cinema>(2)
                .ToArray();

            DbContext.Movies.AddRange(movies);
            DbContext.Cinemas.AddRange(cinemas);
            DbContext.SaveChanges();

            var movieShowtimes = _fixture.CreateMany<MovieShowtime>(60)
                .ToArray();

            for (var i = 0; i < movies.Length; i++)
            {
                movieShowtimes[i].Cinema = cinemas[0];
                movieShowtimes[i].Movie = movies[i];
            }

            for (var i = movieShowtimes.Length - movies.Length; i < movieShowtimes.Length; i++)
            {
                movieShowtimes[i].Cinema = cinemas[1];
                movieShowtimes[i].Movie = movies[i % movies.Length];
            }

            DbContext.MovieShowtimes.AddRange(movieShowtimes);
            DbContext.SaveChanges();
        }

        private void CustomizeFixture()
        {
            _fixture.Customize(new MovieDatabaseFixtureCustomization());
        }

        public void Dispose()
        {
            if (DbContext.Database.EnsureDeleted())
                _logger.LogInformation("Database was deleted");

            DbContext?.Dispose();
        }
    }
}