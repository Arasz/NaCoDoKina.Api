using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.DataProviders.CinemaCity.Movies.Tasks;
using Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Settings.CinemaNetwork;
using Xunit;
using CinemaNetwork = ApplicationCore.Entities.Cinemas.CinemaNetwork;

namespace HangfireHost.Integration.Tests.DataProviders
{
    public class CinemaCityMoviesTaskTest : TaskTestBase<LoadCinemaCityMoviesTask>
    {
        [Fact]
        public async Task Should_execute_task_and_save_all_movies_from_cinema_city_to_database()
        {
            // Arrange
            var context = GetDbContext<ApplicationContext>();

            var moviesCount = context.Movies.Count();

            var networksSettings = Services.GetService<CinemaNetworksSettings>();
            var cinemaCityNetwork = new CinemaNetwork
            {
                Name = networksSettings.CinemaCityNetwork.Name,
                CinemaNetworkUrl = networksSettings.CinemaCityNetwork.Url
            };

            context.CinemaNetworks.Add(cinemaCityNetwork);
            await context.SaveChangesAsync();

            // Act

            await TaskUnderTest.Execute();

            // Assert

            var newCount = context.Movies.Count();

            newCount.Should().BeGreaterThan(moviesCount);
        }
    }
}