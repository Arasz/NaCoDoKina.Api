using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NaCoDoKina.Api.IntegrationTests.Api.DatabaseSeed;
using NaCoDoKina.Api.IntegrationTests.Api.Fixtures;

namespace NaCoDoKina.Api.IntegrationTests.Api
{
    public class HttpTestWithDatabase : HttpTestBase
    {
        protected TDbContext GetDbContext<TDbContext>()
            where TDbContext : DbContext
        {
            return Services.GetService<TDbContext>();
        }

        public HttpTestWithDatabase()
        {
            var databaseFixture = Services.GetService<DatabaseFixture>();
            databaseFixture.CreateDatabase();
        }

        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            base.ConfigureServices(serviceCollection);

            serviceCollection.AddSingleton<IDatabaseSeed, ApplicationDataSeed>();
            serviceCollection.AddSingleton<IDatabaseSeed, IdentityDataSeed>();
            serviceCollection.AddSingleton<DatabaseFixture>();
        }
    }
}