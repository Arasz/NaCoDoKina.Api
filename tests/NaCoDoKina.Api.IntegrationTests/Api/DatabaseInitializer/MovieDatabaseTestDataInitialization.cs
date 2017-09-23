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

        private readonly IFixture _fixture;

        public MovieDatabaseTestDataInitialization(ApplicationContext applicationContext, ILogger<MovieDatabaseTestDataInitialization> logger, IFixture fixture)
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

            _logger.LogInformation("Generating and saving test data");

            await GenerateAndSaveData();

            _logger.LogInformation("Initialization complete");
        }

        private async Task GenerateAndSaveData()
        {
            var movies = _fixture.CreateMany<Movie>(15)
                .ToArray();

            var cinemas = _fixture.CreateMany<Cinema>(3)
                .ToArray();

            var movieShowtimes = _fixture.CreateMany<MovieShowtime>(15)
                .ToArray();

            var rnd = new Random(5);
            for (var i = 0; i < movieShowtimes.Length; i++)
            {
                movieShowtimes[i].Cinema = cinemas[rnd.Next(0, cinemas.Length)];
                movieShowtimes[i].Movie = movies[i];
            }

            DbContext.Cinemas.AddRange(cinemas);
            DbContext.MovieShowtimes.AddRange(movieShowtimes);
            await DbContext.SaveChangesAsync();
        }

        private void CustomizeFixture()
        {
            _fixture.Customize(new MovieDatabaseFixtureCustomization());
        }
    }
}