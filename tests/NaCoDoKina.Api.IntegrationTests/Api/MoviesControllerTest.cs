using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.IntegrationTests.Api.DatabaseInitializer;
using Ploeh.AutoFixture;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.IntegrationTests.Api
{
    public class MoviesControllerTest : HttpTestBase
    {
        private IDbInitialize<ApplicationContext> _dbInitialize;

        public MoviesControllerTest()
        {
            _dbInitialize = Services.GetService(typeof(IDbInitialize<ApplicationContext>)) as IDbInitialize<ApplicationContext>;
        }

        protected override void ConfigureServices(WebHostBuilderContext webHostBuilderContext,
            IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<Fixture>();
            serviceCollection.AddTransient<IDbInitialize<ApplicationContext>, MovieDatabaseTestDataInitialization>();
        }

        public class GetAllMoviesAsync : MoviesControllerTest
        {
            [Fact]
            public async Task Should_return_all_movies_inside_search_area()
            {
                // Arrange
                await _dbInitialize.InitializeAsync();
            }
        }
    }
}