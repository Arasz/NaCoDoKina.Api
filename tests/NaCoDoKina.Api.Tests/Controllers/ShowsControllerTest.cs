using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NaCoDoKina.Api.Controllers;
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
        public async void Should_retutn_OkObjectResult_with_all_accessible_shows()
        {
            //Arrange
            var expectedShowsIds = new[] { 1, 2, 3 };

            //Act
            var result = await ControllerUnderTest.GetAllAsync();

            //Assert
            var okResult = result.Should().BeOfType<OkObjectResult>()
                .As<OkObjectResult>();
            okResult.Value.Should().BeSameAs(expectedShowsIds);
        }
    }
}