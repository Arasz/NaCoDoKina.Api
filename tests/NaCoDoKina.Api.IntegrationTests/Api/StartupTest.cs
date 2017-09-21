using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NaCoDoKina.Api.Services;
using Xunit;

namespace NaCoDoKina.Api.IntegrationTests.Api
{
    public class StartupTest : HttpTestBase
    {
        public class ConfigureServices : StartupTest
        {
            [Fact]
            public void Should_return_cinema_service()
            {
                // Arrange
                var serviceProvider = Server.Host.Services;

                // Act
                var cinemaService = serviceProvider.GetService<ICinemaService>();

                // Assert
                cinemaService.Should().NotBeNull();
            }
        }
    }
}