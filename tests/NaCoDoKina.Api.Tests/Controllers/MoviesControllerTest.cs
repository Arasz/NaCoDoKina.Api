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

        protected Mock<IMovieService> ShowServiceMock { get; set; }

        protected MoviesController ControllerUnderTest { get; set; }

        public MoviesControllerTest()
        {
            LoggerMock = new Mock<ILogger<MoviesController>>();
            ShowServiceMock = new Mock<IMovieService>();
            ControllerUnderTest = new MoviesController(ShowServiceMock.Object, LoggerMock.Object);
        }
    }

    public class GetAllMoviesAsync : MoviesControllerTest
    {
        [Fact]
        public async void Should_return_OkObjectResult_with_all_accessible_shows()
        {
            //Arrange
            var expectedShowsIds = new[] { 1L, 2L, 3L };
            var userLocation = new Location { Longitude = 1, Latitude = 1 };

            ShowServiceMock
                .Setup(service => service.GetAllMoviesAsync(userLocation))
                .Returns(() => Task.FromResult(expectedShowsIds.AsEnumerable()));

            //Act
            var result = await ControllerUnderTest.GetAllMoviesAsync(userLocation);

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
            var userLocation = new Location { Longitude = 1, Latitude = 1 };

            ShowServiceMock
                .Setup(service => service.GetAllMoviesAsync(userLocation))
                .ThrowsAsync(new ShowsNotFoundException());

            //Act
            var result = await ControllerUnderTest.GetAllMoviesAsync(userLocation);

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
            var show = new Movie();

            ShowServiceMock
                .Setup(service => service.GetMovieAsync(showId))
                .Returns(() => Task.FromResult(show));

            //Act
            var result = await ControllerUnderTest.GetMovieAsync(showId);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeSameAs(show);
        }

        [Fact]
        public async void Should_return_NotFoundResult_when_ShowNotFoundException_is_thrown()
        {
            //Arrange
            var unexistingShowId = -5;

            ShowServiceMock
                .Setup(service => service.GetMovieAsync(unexistingShowId))
                .ThrowsAsync(new ShowNotFoundException());

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

            ShowServiceMock
                .Setup(service => service.DeleteMovieAsync(deletedShowId))
                .Returns(() => Task.CompletedTask)
                .Verifiable();

            ShowServiceMock
                .Setup(service => service.GetMovieAsync(deletedShowId))
                .ThrowsAsync(new ShowNotFoundException());

            //Act
            var result = await ControllerUnderTest.DeleteMovieAsync(deletedShowId);
            var getResult = await ControllerUnderTest.GetMovieAsync(deletedShowId);

            //Assert
            result.Should().BeOfType<OkResult>();
            getResult.Should().BeOfType<NotFoundObjectResult>();
            ShowServiceMock.Verify();
        }

        [Fact]
        public async void Should_return_NotFoundResult_when_ShowNotFoundException_is_thrown()
        {
            //Arrange
            var unexistingShowId = -5;

            ShowServiceMock
                .Setup(service => service.DeleteMovieAsync(unexistingShowId))
                .ThrowsAsync(new ShowNotFoundException());

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
            var showDetails = new MovieDetails();

            ShowServiceMock
                .Setup(service => service.GetMovieDetailsAsync(showId))
                .Returns(() => Task.FromResult(showDetails));

            //Act
            var result = await ControllerUnderTest.GetMovieDetailsAsync(showId);

            //Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult.Value.Should().BeSameAs(showDetails);
        }

        [Fact]
        public async void Should_return_NotFoundResult_when_ShowDetailsNotFoundException_is_thrown()
        {
            //Arrange
            var unexistingShowDetailsId = -5;

            ShowServiceMock
                .Setup(service => service.GetMovieDetailsAsync(unexistingShowDetailsId))
                .ThrowsAsync(new ShowDetailsNotFoundException());

            //Act
            var result = await ControllerUnderTest.GetMovieDetailsAsync(unexistingShowDetailsId);

            //Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}