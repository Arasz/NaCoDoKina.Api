using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.DataProviders.Common.CinemaNetworks;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace HangfireHost.Integration.Tests.DataProviders
{
    public class LoadCinemaNetworksFromConfigurationTaskTest : TaskTestBase<LoadCinemaNetworksFromConfigurationTask>
    {
        [Fact]
        public async Task Should_execute_task_and_save_all_cinema_networks_from_configuration_to_database()
        {
            // Arrange
            var context = GetDbContext<ApplicationContext>();

            var count = context.CinemaNetworks.Count();

            // Act

            await TaskUnderTest.Execute();

            // Assert

            var newCount = context.CinemaNetworks.Count();

            newCount.Should().BeGreaterThan(count);
        }
    }
}