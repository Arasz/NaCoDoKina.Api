using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.DataContracts.Movies;
using NaCoDoKina.Api.IntegrationTests.Api.DatabaseInitializer;
using NaCoDoKina.Api.IntegrationTests.Api.Extensions;
using System;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.IntegrationTests.Api
{
    public class MoviesControllerTest : HttpTestBase
    {
        private readonly IDbInitialize<ApplicationContext> _dbInitialize;

        public MoviesControllerTest()
        {
            _dbInitialize = Services.GetService(typeof(IDbInitialize<ApplicationContext>)) as IDbInitialize<ApplicationContext>;
        }

        protected override void ConfigureServices(IServiceCollection serviceCollection)
        {
            base.ConfigureServices(serviceCollection);
            serviceCollection.AddTransient<IDbInitialize<ApplicationContext>, MovieDatabaseTestDataInitialization>();
        }

        public class GetAllMoviesAsync : MoviesControllerTest
        {
            private string ParseSearchAreaToQuery(SearchArea searchArea)
            {
                var queryString = new StringBuilder("?");

                queryString.Append($"{nameof(SearchArea.Center)}.{nameof(Location.Longitude)}=" +
                                   $"{searchArea.Center.Longitude}&");
                queryString.Append($"{nameof(SearchArea.Center)}.{nameof(Location.Latitude)}=" +
                                   $"{searchArea.Center.Latitude}&");
                queryString.Append($"{nameof(SearchArea.Radius)}={searchArea.Radius}");

                return queryString.ToString();
            }

            [Fact]
            public async Task Should_return_all_movies_inside_search_area()
            {
                // Arrange
                await _dbInitialize.InitializeAsync();

                var testLocation = new Location(52.44056, 16.919235);
                var searchArea = new SearchArea(testLocation, 100);

                // Act
                var url = $"{Version}/movies/{ParseSearchAreaToQuery(searchArea)}";
                var response = await Client.GetAsync(url);

                // Assert

                Action action = () => response.EnsureSuccessStatusCode();
                action.ShouldNotThrow();

                var responseContent = await response.Content.ReadAsJsonObjectAsync<long[]>();
                responseContent
                    .Should()
                    .NotBeNullOrEmpty().And
                    .HaveCount(3);
            }
        }
    }
}