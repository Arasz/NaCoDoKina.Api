using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NaCoDoKina.Api.DataContracts;
using NaCoDoKina.Api.Exceptions;
using NaCoDoKina.Api.Services;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Controllers
{
    /// <summary>
    /// Base class for all show controller tests 
    /// </summary>
    public class MoviesControllerTest
    {
        protected Mock<ILogger<MoviesController>> LoggerMock { get; set; }

        protected Mock<IMovieService> MovieServiceMock { get; set; }

        protected Mock<IMapper> MapperMock { get; set; }

        protected Mock<ICinemaService> CinemaServiceMock { get; set; }

        protected MoviesController ControllerUnderTest { get; set; }

        public MoviesControllerTest()
        {
            LoggerMock = new Mock<ILogger<MoviesController>>();
            MovieServiceMock = new Mock<IMovieService>();
            ControllerUnderTest = new MoviesController(MovieServiceMock.Object, CinemaServiceMock.Object, LoggerMock.Object, MapperMock.Object);
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
            var apiLocation = new Location(1, 1);

            MapperMock
                .Setup(mapper => mapper.Map<Models.Location>(apiLocation))
                .Returns(modelLocation);

            MovieServiceMock
                .Setup(service => service.GetAllMoviesAsync(modelLocation))
                .Returns(() => Task.FromResult(expectedShowsIds.AsEnumerable()));

            //Act
            var result = await ControllerUnderTest.GetAllMoviesAsync(apiLocation);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult?.Value.Should().BeSameAs(expectedShowsIds);
        }

        [Fact]
        public async void Should_return_BadRequestResult_when_location_is_null()
        {
            //Arrange
            Location userLocation = null;

            //Act
            var result = await ControllerUnderTest.GetAllMoviesAsync(userLocation);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async void Should_return_NotFoundResult_when_ShowsNotFoundException_is_thrown()
        {
            //Arrange
            var expectedShowsIds = new[] { 1L, 2L, 3L };
            var modelLocation = new Models.Location(1, 1);
            var apiLocation = new Location(1, 1);

            MapperMock
                .Setup(mapper => mapper.Map<Models.Location>(apiLocation))
                .Returns(modelLocation);

            MovieServiceMock
                .Setup(service => service.GetAllMoviesAsync(modelLocation))
                .ThrowsAsync(new MoviesNotFoundException(expectedShowsIds));

            //Act
            var result = await ControllerUnderTest.GetAllMoviesAsync(apiLocation);

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
                .Setup(mapper => mapper.Map<Movie>(It.IsAny<Models.Movie>()))
                .Returns(movie);

            MovieServiceMock
                .Setup(service => service.GetMovieAsync(showId))
                .Returns(() => Task.FromResult(new Models.Movie()));

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
                .Setup(mapper => mapper.Map<MovieDetails>(It.IsAny<Models.MovieDetails>()))
                .Returns(movieDetails);

            MovieServiceMock
                .Setup(service => service.GetMovieDetailsAsync(showId))
                .Returns(() => Task.FromResult(new Models.MovieDetails()));

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
}