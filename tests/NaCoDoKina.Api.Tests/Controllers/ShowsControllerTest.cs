using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NaCoDoKina.Api.Controllers;
using NaCoDoKina.Api.DataContracts;
using Xunit;

namespace NaCoDoKina.Api.Tests.Controllers
{
    /// <summary>
    /// Base class for all show controller tests 
    /// </summary>
    public class ShowsControllerTest
    {
        protected ShowsController ControllerUnderTest { get; set; }

        public ShowsControllerTest()
        {
            ControllerUnderTest = new ShowsController();
        }
    }

    public class GetAllAsync : ShowsControllerTest
    {
        [Fact]
        public async void Should_return_OkObjectResult_with_all_accessible_shows()
        {
            //Arrange
            var expectedShowsIds = new[] { 1, 2, 3 };
            var userLocation = new Location { Longitude = 1, Latitude = 1 };

            //Act
            var result = await ControllerUnderTest.GetAllAsync(userLocation);

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
            var result = await ControllerUnderTest.GetAllAsync(userLocation);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async void Should_return_NotFoundResult_when_there_are_no_shows_in_the_neighborhood()
        {
            //Arrange
            var userLocation = new Location { Longitude = 1, Latitude = 1 };

            //Act
            var result = await ControllerUnderTest.GetAllAsync(userLocation);

            //Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}