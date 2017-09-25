using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NaCoDoKina.Api.DataContracts.Authentication;
using NaCoDoKina.Api.DataContracts.Movies;
using NaCoDoKina.Api.Infrastructure.Identity;
using NaCoDoKina.Api.IntegrationTests.Api.Extensions;
using System;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.IntegrationTests.Api
{
    public class MoviesControllerTest : HttpTestWithDatabase
    {
        /// <summary>
        /// Logins as default user and returns token 
        /// </summary>
        /// <returns></returns>
        public async Task<string> Login()
        {
            var user = await GetDbContext<ApplicationIdentityContext>().Users.FirstAsync();
            var url = $"{ApiSettings.Version}/users/token";
            var payload = new Credentials
            {
                UserName = user.UserName,
                Password = ApiSettings.DefaultUserPassword
            };

            var response = await Client.PostAsync(url, GetPayload(payload));

            var token = await response.Content.ReadAsJsonObjectAsync<JwtToken>();

            return token.Token;
        }

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

                var testLocation = new Location(51.44056, 16.919235);
                var searchArea = new SearchArea(testLocation, 100);
                var queryString = ParseSearchAreaToQuery(searchArea);

                // Act

                var token = await Login();
                var url = $"{ApiSettings.Version}/movies/{queryString}";
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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