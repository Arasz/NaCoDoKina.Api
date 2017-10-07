using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.DataProviders.CinemaCity.Movies.Tasks;
using NaCoDoKina.Api.Infrastructure.Settings;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using CinemaNetwork = NaCoDoKina.Api.Entities.Cinemas.CinemaNetwork;

namespace NaCoDoKina.Api.IntegrationTests.DataProviders
{
    public class MoviesTaskTest : TaskTestBase<MoviesTask>
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