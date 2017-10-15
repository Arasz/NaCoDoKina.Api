using FluentAssertions;
using Infrastructure.Identity;
using IntegrationTestsCore;
using IntegrationTestsCore.Extensions;
using NaCoDoKina.Api.DataContracts;
using NaCoDoKina.Api.DataContracts.Authentication;
using NaCoDoKina.Api.DataContracts.Cinemas;
using NaCoDoKina.Api.DataContracts.Movies;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.IntegrationTests.Api
{
    public class MoviesControllerTest : HttpTestWithDatabase<Startup>
    {
        /// <summary>
        /// Login as user determined by its position and add authorization header with token 
        /// </summary>
        /// <returns></returns>
        protected async Task Login(int userPosition = 0)
        {
            var user = GetDbContext<ApplicationIdentityContext>().Users
                .Take(userPosition + 1)
                .ToArray()
                .ElementAtOrDefault(userPosition);

            if (user is null)
                throw new ArgumentException($"There is no user at position {userPosition}");

            var url = $"{TestsSettings.Version}/auth/token";
            var payload = new Credentials
            {
                UserName = user.UserName,
                Password = TestsSettings.DefaultUserPassword
            };

            var response = await Client.PostAsync(url, GetPayload(payload));

            var token = await HttpContentExtensions.ReadAsJsonObjectAsync<JwtToken>(response.Content);

            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Token);
        }

        /// <summary>
        /// Parses search area to query string 
        /// </summary>
        /// <param name="searchArea"></param>
        /// <returns></returns>
        protected string ParseSearchAreaToQuery(SearchArea searchArea)
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

        /// <summary>
        /// Base url for movie controller 
        /// </summary>
        protected string BaseUrl => $"{TestsSettings.Version}/movies/";

        public class GetAllMoviesAsync : MoviesControllerTest
        {
            [Fact]
            public async Task Should_return_all_movies_inside_search_area()
            {
                // Arrange

                var testLocation = new Location(52.3886399802915, 16.8517549802915);
                var searchArea = new SearchArea(testLocation, 10000);
                var queryString = ParseSearchAreaToQuery(searchArea);

                // Act

                await Login();
                var url = $"{BaseUrl}{queryString}";
                var response = await Client.GetAsync(url);

                // Assert

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsJsonObjectAsync<long[]>();
                responseContent
                    .Should()
                    .NotBeNullOrEmpty().And
                    .OnlyHaveUniqueItems().And
                    .Subject.Count().Should().BePositive();
            }

            [Fact]
            public async Task Should_return_all_movies_inside_search_area_without_login()
            {
                // Arrange

                var testLocation = new Location(52.3886399802915, 16.8517549802915);
                var searchArea = new SearchArea(testLocation, 10000);
                var queryString = ParseSearchAreaToQuery(searchArea);

                // Act

                var url = $"{BaseUrl}{queryString}";
                var response = await Client.GetAsync(url);

                // Assert

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsJsonObjectAsync<long[]>();
                responseContent
                    .Should()
                    .NotBeNullOrEmpty().And
                    .OnlyHaveUniqueItems().And
                    .Subject.Count().Should().BePositive();
            }
        }

        public class GetMovieAsync : MoviesControllerTest
        {
            [Fact]
            public async Task Should_return_movie_with_given_id_when_id_is_correct()
            {
                // Arrange
                var movieId = 1L;
                var url = $"{BaseUrl}{movieId}";

                // Act
                await Login();
                var response = await Client.GetAsync(url);

                // Assert
                response.EnsureSuccessStatusCode();
                var movie = await response.Content.ReadAsJsonObjectAsync<Movie>();

                movie.Id.Should().Be(movieId);
                movie.Title.Should().NotBeNullOrEmpty();
            }

            [Fact]
            public async Task Should_return_not_found_status_code_when_id_is_incorrect()
            {
                // Arrange
                var movieId = 999999;
                var url = $"{BaseUrl}{movieId}";

                // Act
                await Login();
                var response = await Client.GetAsync(url);

                // Assert
                response.StatusCode.Should().HaveFlag(HttpStatusCode.NotFound);
                var content = await response.Content.ReadAsStringAsync();
                content.Should().NotBeNullOrEmpty();
            }
        }

        public class DeleteMovieAsync : MoviesControllerTest
        {
            [Fact]
            public async Task Should_delete_movie_only_for_one_user()
            {
                // Arrange
                var movieId = 1L;
                var url = $"{BaseUrl}{movieId}";

                // Login as first user and delete
                await Login();
                var response = await Client.DeleteAsync(url);

                response.EnsureSuccessStatusCode();

                // Check if deleted for first user
                var getResponse = await Client.GetAsync(url);
                getResponse.StatusCode.Should().HaveFlag(HttpStatusCode.NotFound);

                // Login as second user, should get movie
                await Login(1);
                var movieResponse = await Client.GetAsync(url);

                movieResponse.EnsureSuccessStatusCode();
                var movie = await movieResponse.Content.ReadAsJsonObjectAsync<Movie>();

                movie.Id.Should().Be(movieId);
                movie.Title.Should().NotBeNullOrEmpty();
            }
        }

        public class GetMovieDetailsAsync : MoviesControllerTest
        {
            [Fact]
            public async Task Should_return_movie_details_with_given_id_when_id_is_correct()
            {
                // Arrange
                var movieId = 1L;
                var url = $"{BaseUrl}{movieId}/details";

                // Act
                await Login();
                var response = await Client.GetAsync(url);

                // Assert
                response.EnsureSuccessStatusCode();
                var movieDetails = await response.Content.ReadAsJsonObjectAsync<MovieDetails>();

                movieDetails.Id.Should().Be(movieId);
                movieDetails.Title.Should().NotBeNullOrEmpty();
                movieDetails.MediaResources.Should().NotBeNullOrEmpty();
                movieDetails.MovieReviews.Should().NotBeNullOrEmpty();
                movieDetails.Rating.Should().BeGreaterThan(0); // Default rating is greater than 0
            }

            [Fact]
            public async Task Should_return_bad_request_when_id_is_less_than_or_equal_0()
            {
                // Arrange
                var movieId = -10;
                var url = $"{BaseUrl}{movieId}/details";

                // Act
                await Login();
                var response = await Client.GetAsync(url);

                // Assert
                response.StatusCode.Should().HaveFlag(HttpStatusCode.BadRequest);
            }
        }

        public class SetRatingForMovie : MoviesControllerTest
        {
            [Fact]
            public async Task Should_set_rating_for_movie()
            {
                // Arrange
                var movieId = 1L;
                var url = $"{BaseUrl}{movieId}/rating";
                var rating = Fixture.Create<double>();

                // Act
                await Login();
                var response = await Client.PostAsync(url, GetPayload(rating));

                // Assert
                response.StatusCode.Should().HaveFlag(HttpStatusCode.Created);

                var returnedRating = await response.Content.ReadAsJsonObjectAsync<double>();

                returnedRating.Should().Be(returnedRating);
            }
        }

        public class GetMovieShowtimesAsync : MoviesControllerTest
        {
            [Fact]
            public async Task Should_return_all_showtimes_for_movie_in_given_cinema()
            {
                // Arrange
                var movieId = 1L;
                var now = DateTime.Now.AddMinutes(-2);
                var cinemaId = 1;
                var url = $"{BaseUrl}{movieId}/cinemas/{cinemaId}/showtimes";

                // Act
                await Login();

                var response = await Client.GetAsync(url);

                // Assert
                response.EnsureSuccessStatusCode();

                var movieShowtimes = await response.Content.ReadAsJsonObjectAsync<MovieShowtime[]>();

                movieShowtimes.Should().NotBeNullOrEmpty();
                movieShowtimes.Should().Match(showtimes => showtimes.All(showtime => showtime.MovieId == movieId));
                movieShowtimes.Should().Match(showtimes => showtimes.All(showtime => showtime.ShowTime > now));
            }

            [Fact]
            public async Task Should_return_all_showtimes_for_movie_after_given_time()
            {
                // Arrange
                var movieId = 1L;
                var now = DateTime.Now.AddDays(-2);
                var cinemaId = 1;
                var url = $"{BaseUrl}{movieId}/cinemas/{cinemaId}/showtimes/?laterThan={now.ToString(CultureInfo.InvariantCulture)}";

                // Act
                await Login();

                var response = await Client.GetAsync(url);

                // Assert
                response.EnsureSuccessStatusCode();

                var movieShowtimes = await response.Content.ReadAsJsonObjectAsync<MovieShowtime[]>();

                movieShowtimes.Should().NotBeNullOrEmpty();
                movieShowtimes.Should().Match(showtimes => showtimes.All(showtime => showtime.MovieId == movieId));
                movieShowtimes.Should().Match(showtimes => showtimes.All(showtime => showtime.ShowTime > now));
            }
        }

        public class GetCinemasPlayingMovieInSearchArea : MoviesControllerTest
        {
            [Fact]
            public async Task Should_return_all_cinemas_inside_search_area()
            {
                // Arrange

                var testLocation = new Location(52.3886399802915, 16.8517549802915);
                var searchArea = new SearchArea(testLocation, 10000);
                var queryString = ParseSearchAreaToQuery(searchArea);
                var movieId = 1L;

                // Act

                await Login();
                var url = $"{BaseUrl}{movieId}/cinemas/{queryString}";
                var response = await Client.GetAsync(url);

                // Assert

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsJsonObjectAsync<List<Cinema>>();

                responseContent
                    .Should()
                    .NotBeNullOrEmpty().And
                    .OnlyHaveUniqueItems().And
                    .Subject.Count().Should().BePositive();
            }

            [Fact]
            public async Task Should_return_all_cinemas_inside_search_area_without_login()
            {
                // Arrange

                var testLocation = new Location(52.3886399802915, 16.8517549802915);
                var searchArea = new SearchArea(testLocation, 10000);
                var queryString = ParseSearchAreaToQuery(searchArea);
                var movieId = 1L;

                // Act

                var url = $"{BaseUrl}{movieId}/cinemas/{queryString}";
                var response = await Client.GetAsync(url);

                // Assert

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsJsonObjectAsync<List<Cinema>>();

                responseContent
                    .Should()
                    .NotBeNullOrEmpty().And
                    .OnlyHaveUniqueItems().And
                    .Subject.Count().Should().BePositive();
            }
        }
    }
}