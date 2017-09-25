using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NaCoDoKina.Api.IntegrationTests.Api.DatabaseSeed;
using NaCoDoKina.Api.IntegrationTests.Api.Fixtures;
using Xunit;

namespace NaCoDoKina.Api.IntegrationTests.Api
{
    [Collection("Database tests")]
    public class HttpTestWithDatabase : HttpTestBase
    {
        private readonly DatabaseFixture _databaseFixture;

        protected TDbContext GetDbContext<TDbContext>()
            where TDbContext : DbContext
        {
            return Services.GetService<TDbContext>();
        }

        public HttpTestWithDatabase()
        {
            _databaseFixture = Services.GetService<DatabaseFixture>();
            _databaseFixture.CreateDatabase();
        }

        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            base.ConfigureServices(serviceCollection);

            serviceCollection.AddSingleton<IDatabaseSeed, ApplicationDataSeed>();
            serviceCollection.AddSingleton<IDatabaseSeed, IdentityDataSeed>();
            serviceCollection.AddSingleton<DatabaseFixture>();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                _databaseFixture.Dispose();
            }
        }
    }
}