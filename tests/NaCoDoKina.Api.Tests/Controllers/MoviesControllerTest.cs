using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NaCoDoKina.Api.DataContracts;
using NaCoDoKina.Api.DataContracts.Movies;
using NaCoDoKina.Api.Exceptions;
using NaCoDoKina.Api.Results;
using NaCoDoKina.Api.Services;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Cinema = NaCoDoKina.Api.DataContracts.Cinemas.Cinema;
using Location = NaCoDoKina.Api.DataContracts.Location;
using Movie = NaCoDoKina.Api.DataContracts.Movies.Movie;
using MovieDetails = NaCoDoKina.Api.DataContracts.Movies.MovieDetails;

namespace NaCoDoKina.Api.Controllers
{
    public class MoviesControllerTest : UnitTestBase
    {
        protected Mock<ILogger<MoviesController>> LoggerMock { get; set; }

        protected Mock<IMovieService> MovieServiceMock { get; set; }

        protected Mock<IMapper> MapperMock { get; set; }

        protected Mock<ICinemaService> CinemaServiceMock { get; set; }

        protected Mock<IRatingService> RatingServiceMock { get; set; }

        protected MoviesController ControllerUnderTest { get; set; }

        protected Mock<IMovieShowtimeService> MovieShowtimeServiceMock { get; set; }

        public MoviesControllerTest()
        {
            RatingServiceMock = new Mock<IRatingService>();
            CinemaServiceMock = new Mock<ICinemaService>();
            MapperMock = new Mock<IMapper>();
            LoggerMock = new Mock<ILogger<MoviesController>>();
            MovieServiceMock = new Mock<IMovieService>();
            MovieShowtimeServiceMock = new Mock<IMovieShowtimeService>();
            ControllerUnderTest = new MoviesController(MovieServiceMock.Object, CinemaServiceMock.Object,
                RatingServiceMock.Object, LoggerMock.Object, MapperMock.Object,
                MovieShowtimeServiceMock.Object);
        }

        public class GetMovieShowtimesAsync : MoviesControllerTest
        {
            [Fact]
            public async Task Should_return_OkObjectResult_with_all_movie_show_times_for_all_cinemas()
            {
                // Arrange
                var movieId = Fixture.Create<long>();
                var cinemaId = Fixture.Create<long>();
                var moviesCount = 5;
                var movieShowtimes = Fixture.CreateMany<Models.Movies.MovieShowtime>(moviesCount);
                var laterThan = Fixture.Create<DateTime>();

                MapperMock.Setup(mapper => mapper.Map<MovieShowtime>(It.IsAny<Models.Movies.MovieShowtime>()))
                    .Returns(new Func<Models.Movies.MovieShowtime, MovieShowtime>(showtime => new MovieShowtime
                    {
                        ShowType = showtime.ShowType,

                        Language = showtime.Language,
                        ShowTime = showtime.ShowTime,
                    }));

                MovieShowtimeServiceMock
                    .Setup(service => service.GetMovieShowtimesForCinemaAsync(movieId, cinemaId, laterThan))
                    .ReturnsAsync(movieShowtimes);

                // Act
                var result = await ControllerUnderTest.GetMovieShowtimesAsync(movieId, cinemaId, laterThan);

                //Assert
                result.Should().BeOfType<OkObjectResult>();
                var okResult = result as OkObjectResult;
                (okResult.Value as IEnumerable<MovieShowtime>)
                    .Should().HaveSameCount(movieShowtimes);
            }

            [Fact]
            public async Task Should_return_OkObjectResult_with_all_movie_show_times_for_given_cinema()
            {
                // Arrange
                var cinemaId = Fixture.Create<long>();
                var movieId = Fixture.Create<long>();
                var moviesCount = 5;
                var laterThan = Fixture.Create<DateTime>();
                var movieShowtimes = Fixture.CreateMany<Models.Movies.MovieShowtime>(moviesCount);

                MapperMock.Setup(mapper => mapper.Map<MovieShowtime>(It.IsAny<Models.Movies.MovieShowtime>()))
                    .Returns(new Func<Models.Movies.MovieShowtime, MovieShowtime>(showtime => new MovieShowtime
                    {
                        ShowType = showtime.ShowType,

                        Language = showtime.Language,
                        ShowTime = showtime.ShowTime,
                    }));

                MovieShowtimeServiceMock
                    .Setup(service => service.GetMovieShowtimesForCinemaAsync(movieId, cinemaId, laterThan))
                    .ReturnsAsync(movieShowtimes);

                // Act
                var result = await ControllerUnderTest.GetMovieShowtimesAsync(movieId, cinemaId, laterThan);

                //Assert
                result.Should().BeOfType<OkObjectResult>();
                var okResult = result as OkObjectResult;
                (okResult.Value as IEnumerable<MovieShowtime>)
                    .Should().HaveSameCount(movieShowtimes);
            }

            [Fact]
            public async Task Should_return_NotFoundObjectResult_when_movie_does_not_have_any_showtime()
            {
                // Arrange
                var cinemaId = Fixture.Create<long>();
                var movieId = Fixture.Create<long>();
                var laterThan = Fixture.Create<DateTime>();

                MapperMock.Setup(mapper => mapper.Map<MovieShowtime>(It.IsAny<MovieShowtime>()))
                    .Returns(new Func<Models.Movies.MovieShowtime, MovieShowtime>(showtime => new MovieShowtime
                    {
                        ShowType = showtime.ShowType,

                        Language = showtime.Language,
                        ShowTime = showtime.ShowTime,
                    }));

                MovieShowtimeServiceMock
                    .Setup(service => service.GetMovieShowtimesForCinemaAsync(movieId, cinemaId, laterThan))
                    .ThrowsAsync(new MovieShowtimeNotFoundException(movieId, cinemaId));

                // Act
                var result = await ControllerUnderTest.GetMovieShowtimesAsync(movieId, cinemaId, laterThan);

                //Assert
                result.Should().BeOfType<NotFoundObjectResult>();
            }
        }
    }

    public class GetAllMoviesAsync : MoviesControllerTest
    {
        [Fact]
        public async void Should_return_OkObjectResult_with_all_accessible_shows()
        {
            //Arrange
            var expectedShowsIds = new[] { 1L, 2L, 3L };
            var modelLocation = new Models.Location(1, 1);
            var modelSearchArea = new Models.SearchArea(modelLocation, 500);
            var apiLocation = new Location(1, 1);
            var apiSearchArea = new SearchArea(apiLocation, 500);

            MapperMock
                .Setup(mapper => mapper.Map<Models.SearchArea>(apiSearchArea))
                .Returns(modelSearchArea);

            MovieServiceMock
                .Setup(service => service.GetAllMoviesAsync(modelSearchArea))
                .Returns(() => Task.FromResult(expectedShowsIds.AsEnumerable()));

            //Act
            var result = await ControllerUnderTest.GetAllMoviesAsync(apiSearchArea);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeSameAs(expectedShowsIds);
        }

        [Fact]
        public async void Should_return_BadRequestResult_when_location_is_null()
        {
            //Arrange
            SearchArea searchArea = null;

            //Act
            var result = await ControllerUnderTest.GetAllMoviesAsync(searchArea);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async void Should_return_NotFoundResult_when_ShowsNotFoundException_is_thrown()
        {
            //Arrange
            var expectedShowsIds = new[] { 1L, 2L, 3L };
            var modelLocation = new Models.Location(1, 1);
            var modelSearchArea = new Models.SearchArea(modelLocation, 500);
            var apiLocation = new Location(1, 1);
            var apiSearchArea = new SearchArea(apiLocation, 500);

            MapperMock
                .Setup(mapper => mapper.Map<Models.SearchArea>(apiSearchArea))
                .Returns(modelSearchArea);

            MovieServiceMock
                .Setup(service => service.GetAllMoviesAsync(modelSearchArea))
                .ThrowsAsync(new MoviesNotFoundException(expectedShowsIds));

            //Act
            var result = await ControllerUnderTest.GetAllMoviesAsync(apiSearchArea);

            //Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }

    public class GetCinemasPlayingMovieInSearchArea : MoviesControllerTest
    {
        [Fact]
        public async void Should_return_OkObjectResult_and_Cinemas()
        {
            //Arrange
            var movieId = 1;
            var modelCinemas = new List<Models.Cinemas.Cinema> { new Models.Cinemas.Cinema() };
            var modelLocation = new Models.Location(1, 1);
            var modelSearchArea = new Models.SearchArea(modelLocation, 500);
            var apiLocation = new Location(1, 1);
            var apiSearchArea = new SearchArea(apiLocation, 500);

            MapperMock
                .Setup(mapper => mapper.Map<Cinema>(It.IsAny<Models.Cinemas.Cinema>()))
                .Returns(new Cinema());

            MapperMock
                .Setup(mapper => mapper.Map<Models.SearchArea>(apiSearchArea))
                .Returns(modelSearchArea);

            CinemaServiceMock
                .Setup(service => service.GetCinemasPlayingMovieInSearchArea(movieId, modelSearchArea))
                .Returns(() => Task.FromResult(modelCinemas.AsEnumerable()));

            //Act
            var result = await ControllerUnderTest.GetCinemasPlayingMovieInSearchArea(movieId, apiSearchArea);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            var resultValue = okResult?.Value as IEnumerable<Cinema>;
            resultValue?.Should()
                .NotBeNullOrEmpty().And
                .HaveCount(modelCinemas.Count);
        }

        [Fact]
        public async void Should_return_BadRequest_when_location_is_null()
        {
            //Arrange
            var movieId = 1;
            //Act
            var result = await ControllerUnderTest.GetCinemasPlayingMovieInSearchArea(movieId, null);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async void Should_return_NotFound_when_cinema_cannot_be_found()
        {
            //Arrange
            var movieId = -5;
            var modelLocation = new Models.Location(1, 1);
            var modelSearchArea = new Models.SearchArea(modelLocation, 500);
            var apiLocation = new Location(1, 1);
            var apiSearchArea = new SearchArea(apiLocation, 500);

            MapperMock
                .Setup(mapper => mapper.Map<Models.SearchArea>(apiSearchArea))
                .Returns(modelSearchArea);

            CinemaServiceMock
                .Setup(service => service.GetCinemasPlayingMovieInSearchArea(movieId, modelSearchArea))
                .Throws(new CinemasNotFoundException(movieId, modelSearchArea));

            //Act
            var result = await ControllerUnderTest.GetCinemasPlayingMovieInSearchArea(movieId, apiSearchArea);

            //Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }

    public class GetMovieAsync : MoviesControllerTest
    {
        [Fact]
        public async void Should_return_OkObjectResult_and_Show_for_given_id()
        {
            //Arrange
            var showId = 1;
            var movie = new Movie();

            MapperMock
                .Setup(mapper => mapper.Map<Movie>(It.IsAny<Models.Movies.Movie>()))
                .Returns(movie);

            MovieServiceMock
                .Setup(service => service.GetMovieAsync(showId))
                .Returns(() => Task.FromResult(new Models.Movies.Movie()));

            //Act
            var result = await ControllerUnderTest.GetMovieAsync(showId);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeSameAs(movie);
        }

        [Fact]
        public async void Should_return_NotFoundResult_when_ShowNotFoundException_is_thrown()
        {
            //Arrange
            var unexistingShowId = -5;

            MovieServiceMock
                .Setup(service => service.GetMovieAsync(unexistingShowId))
                .ThrowsAsync(new MovieNotFoundException(unexistingShowId));

            //Act
            var result = await ControllerUnderTest.GetMovieAsync(unexistingShowId);

            //Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }

    public class DeleteMovieAsync : MoviesControllerTest
    {
        [Fact]
        public async void Should_return_OkResult_and_delete_the_show_with_given_id()
        {
            //Arrange
            var deletedShowId = 1;

            MovieServiceMock
                .Setup(service => service.DeleteMovieAsync(deletedShowId))
                .Returns(() => Task.CompletedTask)
                .Verifiable();

            MovieServiceMock
                .Setup(service => service.GetMovieAsync(deletedShowId))
                .ThrowsAsync(new MovieNotFoundException(deletedShowId));

            //Act
            var result = await ControllerUnderTest.DeleteMovieAsync(deletedShowId);
            var getResult = await ControllerUnderTest.GetMovieAsync(deletedShowId);

            //Assert
            result.Should().BeOfType<OkResult>();
            getResult.Should().BeOfType<NotFoundObjectResult>();
            MovieServiceMock.Verify();
        }

        [Fact]
        public async void Should_return_NotFoundResult_when_ShowNotFoundException_is_thrown()
        {
            //Arrange
            var unexistingShowId = -5;

            MovieServiceMock
                .Setup(service => service.DeleteMovieAsync(unexistingShowId))
                .ThrowsAsync(new MovieNotFoundException(unexistingShowId));

            //Act
            var result = await ControllerUnderTest.DeleteMovieAsync(unexistingShowId);

            //Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }

    public class GetMovieDetailsAsync : MoviesControllerTest
    {
        [Fact]
        public async void Should_return_OkObjectResult_and_ShowDetails_for_given_id()
        {
            //Arrange
            var showId = 1;
            var movieDetails = new MovieDetails();

            MapperMock
                .Setup(mapper => mapper.Map<MovieDetails>(It.IsAny<Models.Movies.MovieDetails>()))
                .Returns(movieDetails);

            MovieServiceMock
                .Setup(service => service.GetMovieDetailsAsync(showId))
                .Returns(() => Task.FromResult(new Models.Movies.MovieDetails()));

            //Act
            var result = await ControllerUnderTest.GetMovieDetailsAsync(showId);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeSameAs(movieDetails);
        }

        [Fact]
        public async void Should_return_NotFoundResult_when_ShowDetailsNotFoundException_is_thrown()
        {
            //Arrange
            var unexistingShowDetailsId = -5;

            MovieServiceMock
                .Setup(service => service.GetMovieDetailsAsync(unexistingShowDetailsId))
                .ThrowsAsync(new MovieDetailsNotFoundException(unexistingShowDetailsId));

            //Act
            var result = await ControllerUnderTest.GetMovieDetailsAsync(unexistingShowDetailsId);

            //Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }

    public class SetMovieRating : MoviesControllerTest
    {
        [Fact]
        public async void Should_return_CreatedAtActionResult()
        {
            //Arrange
            var movieId = 1;
            var rating = 4.5;

            RatingServiceMock
                .Setup(service => service.SetMovieRating(movieId, rating))
                .ReturnsAsync(Result.Success());

            //Act
            var result = await ControllerUnderTest.SetRatingForMovie(movieId, rating);

            //Assert
            result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async void Should_return_NotFoundResult_when_ShowDetailsNotFoundException_is_thrown()
        {
            //Arrange
            var movieId = 1;
            var rating = 4.5;

            RatingServiceMock
                .Setup(service => service.SetMovieRating(movieId, rating))
                .Throws(new MovieNotFoundException(movieId));

            //Act
            var result = await ControllerUnderTest.SetRatingForMovie(movieId, rating);

            //Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}