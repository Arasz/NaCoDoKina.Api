using FluentAssertions;
using NaCoDoKina.Api.DataContracts.Movies;
using NaCoDoKina.Api.IntegrationTests.Api.Extensions;
using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.IntegrationTests.Api
{
    public class MoviesControllerTest : HttpTestBase
    {
        public class GetAllMoviesAsync : MoviesControllerTest
        {
            private string ParseSearchAreaToQuery(SearchArea searchArea)
            {
                var queryString = new StringBuilder("?");

                queryString.Append($"{nameof(SearchArea.Center)}." +
                                   $"{nameof(Location.Longitude)}=" +
                                   $"{searchArea.Center.Longitude.ToString(CultureInfo.InvariantCulture)}&");
                queryString.Append($"{nameof(SearchArea.Center)}." +
                                   $"{nameof(Location.Latitude)}=" +
                                   $"{searchArea.Center.Latitude.ToString(CultureInfo.InvariantCulture)}&");
                queryString.Append($"{nameof(SearchArea.Radius)}" +
                                   $"={searchArea.Radius.ToString(CultureInfo.InvariantCulture)}");

                return queryString.ToString();
            }

            [Fact]
            public async Task Should_return_all_movies_inside_search_area()
            {
                // Arrange
                await SeedDatabaseAsync();

                var testLocation = new Location(51.44056, 16.919235);
                var searchArea = new SearchArea(testLocation, 100);
                var queryString = ParseSearchAreaToQuery(searchArea);

                // Act
                var url = $"{Version}/movies/{queryString}";
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