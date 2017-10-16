using IntegrationTestsCore.DatabaseSeed;
using IntegrationTestsCore.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTestsCore
{
    [Collection("Database tests")]
    public class HttpTestWithDatabase<TStartup> : HttpTestBase<TStartup>
        where TStartup : class
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

            //serviceCollection.AddSingleton<IDatabaseSeed, ApplicationDataSeed>();
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