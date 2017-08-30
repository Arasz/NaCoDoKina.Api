using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NaCoDoKina.Api.Controllers;
using NaCoDoKina.Api.DataContracts;
using NaCoDoKina.Api.Services;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Tests.Controllers
{
    /// <summary>
    /// Base class for all show controller tests 
    /// </summary>
    public class ShowsControllerTest
    {
        protected Mock<IShowService> ShowServiceMock { get; set; }

        protected ShowsController ControllerUnderTest { get; set; }

        public ShowsControllerTest()
        {
            ShowServiceMock = new Mock<IShowService>();
            ControllerUnderTest = new ShowsController(ShowServiceMock.Object);
        }
    }

    public class GetAllAsync : ShowsControllerTest
    {
        [Fact]
        public async void Should_return_OkObjectResult_with_all_accessible_shows()
        {
            //Arrange
            var expectedShowsIds = new[] { 1L, 2L, 3L };
            var userLocation = new Location { Longitude = 1, Latitude = 1 };

            ShowServiceMock
                .Setup(service => service.GetAllShowsAsync(userLocation))
                .Returns(() => Task.FromResult(expectedShowsIds.AsEnumerable()));

            //Act
            var result = await ControllerUnderTest.GetAllShowsAsync(userLocation);

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>()
                .As<OkObjectResult>();
            okResult.Value.Should().BeSameAs(expectedShowsIds);
        }

        [Fact]
        public async void Should_return_BadRequestResult_when_location_is_null()
        {
            //Arrange
            Location userLocation = null;

            //Act
            var result = await ControllerUnderTest.GetAllShowsAsync(userLocation);

            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async void Should_return_NotFoundResult_when_ShowsNotFoundException_is_thrown()
        {
            //Arrange
            var userLocation = new Location { Longitude = 1, Latitude = 1 };

            //Act
            var result = await ControllerUnderTest.GetAllShowsAsync(userLocation);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }

    public class GetShowAsync : ShowsControllerTest
    {
        [Fact]
        public async void Should_return_OkObjectResult_and_Show_for_given_id()
        {
            //Arrange
            var showId = 1;
            var show = new Show();

            //Act
            var result = await ControllerUnderTest.GetShowAsync(showId);

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>()
                .As<OkObjectResult>();
            okResult.Value.Should().BeSameAs(show);
        }

        [Fact]
        public async void Should_return_NotFoundResult_when_ShowNotFoundException_is_thrown()
        {
            //Arrange
            var unexistingShowId = -5;

            //Act
            var result = await ControllerUnderTest.GetShowAsync(unexistingShowId);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }

    public class DeleteShowAsync : ShowsControllerTest
    {
        [Fact]
        public async void Should_return_OkResult_and_delete_the_show_with_given_id()
        {
            //Arrange
            var deletedShowId = 1;
            var show = new Show();

            //Act
            var result = await ControllerUnderTest.DeleteShowAsync(deletedShowId);
            var getResult = await ControllerUnderTest.GetShowAsync(deletedShowId);

            //Assert
            result.Should().BeOfType<OkResult>();
            getResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void Should_return_NotFoundResult_when_ShowNotFoundException_is_thrown()
        {
            //Arrange
            var unexistingShowId = -5;

            //Act
            var result = await ControllerUnderTest.DeleteShowAsync(unexistingShowId);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }

    public class GetShowDetailsAsync : ShowsControllerTest
    {
        [Fact]
        public async void Should_return_OkObjectResult_and_ShowDetails_for_given_id()
        {
            //Arrange
            var showId = 1;
            var showDetails = new ShowDetails();

            //Act
            var result = await ControllerUnderTest.GetShowDetailsAsync(showId);

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>()
                .As<OkObjectResult>();
            okResult.Value.Should().BeSameAs(showDetails);
        }

        [Fact]
        public async void Should_return_NotFoundResult_when_ShowDetailsNotFoundException_is_thrown()
        {
            //Arrange
            var unexistingShowDetailsId = -5;

            //Act
            var result = await ControllerUnderTest.GetShowDetailsAsync(unexistingShowDetailsId);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}