using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.DataProviders.CinemaCity.Cinemas.Tasks;
using Xunit;
using CinemaNetwork = ApplicationCore.Entities.Cinemas.CinemaNetwork;

namespace NaCoDoKina.Api.IntegrationTests.DataProviders
{
    public class CinemaCityCinemasTaskTest : TaskTestBase<CinemaCityCinemasTask>
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

            await TaskUnderTest.Execute();

            // Assert

            var newCount = context.Cinemas.Count();

            newCount.Should().BeGreaterThan(cinemasCount);

            //var id = BackgroundJob.Enqueue(() => task.Execute());
        }
    }
}