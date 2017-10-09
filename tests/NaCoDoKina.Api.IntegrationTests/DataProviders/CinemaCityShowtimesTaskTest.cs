using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.Settings;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.DataProviders.CinemaCity.Cinemas.Tasks;
using Infrastructure.DataProviders.CinemaCity.Movies.Tasks;
using Infrastructure.DataProviders.CinemaCity.Showtimes.Tasks;
using Xunit;
using CinemaNetwork = ApplicationCore.Entities.Cinemas.CinemaNetwork;

namespace NaCoDoKina.Api.IntegrationTests.DataProviders
{
    public class CinemaCityShowtimesTaskTest : TaskTestBase<CinemaCityShowtimesTask>
    {
        [Fact]
        public async Task Should_save_all_showtimes_for_cinema_city_cinemas_in_provided_time_period()
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

            context.Cinemas.RemoveRange(context.Cinemas);
            await context.SaveChangesAsync();

            var getCinemasTask = Services.GetService<CinemaCityCinemasTask>();

            var getMoviesTask = Services.GetService<CinemaCityMoviesTask>();

            await getCinemasTask.Execute();
            await getMoviesTask.Execute();

            var showtimesCount = context.MovieShowtimes.Count();

            // Act

            await TaskUnderTest.Execute();

            // Assert

            var newShowtimesCount = context.MovieShowtimes.Count();

            newShowtimesCount.Should().BeGreaterThan(showtimesCount);
        }
    }
}