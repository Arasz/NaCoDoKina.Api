using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.DataProviders.CinemaCity.Cinemas.Tasks;
using Infrastructure.Settings.CinemaNetwork;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using CinemaNetwork = ApplicationCore.Entities.Cinemas.CinemaNetwork;

namespace HangfireHost.Integration.Tests.DataProviders
{
    public class CinemaCityCinemasTaskTest : TaskTestBase<LoadCinemaCityCinemasTask>
    {
        [Fact]
        public async Task Should_execute_task_and_save_all_cinemas_from_cinema_city_to_database()
        {
            // Arrange
            var context = GetDbContext<ApplicationContext>();

            var networksSettings = Services.GetService<CinemaNetworksSettings>();
            var cinemaCityNetwork = new CinemaNetwork
            {
                Name = networksSettings.CinemaCityNetwork.Name,
                CinemaNetworkUrl = networksSettings.CinemaCityNetwork.Url
            };

            context.CinemaNetworks.Add(cinemaCityNetwork);

            await context.SaveChangesAsync();

            var cinemasCount = context.Cinemas.Count();

            // Act

            await TaskUnderTest.ExecuteAsync();

            // Assert

            var newCount = context.Cinemas.Count();

            newCount.Should().BeGreaterThan(cinemasCount);
        }
    }
}