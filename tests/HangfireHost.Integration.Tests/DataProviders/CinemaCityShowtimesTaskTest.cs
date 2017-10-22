using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.DataProviders.CinemaCity.Cinemas.Tasks;
using Infrastructure.DataProviders.CinemaCity.Movies.Tasks;
using Infrastructure.DataProviders.CinemaCity.Showtimes.Tasks;
using Infrastructure.Settings.CinemaNetwork;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using CinemaNetwork = ApplicationCore.Entities.Cinemas.CinemaNetwork;

namespace HangfireHost.Integration.Tests.DataProviders
{
    public class CinemaCityShowtimesTaskTest : TaskTestBase<LoadCinemaCityShowtimesTask>
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

            var getCinemasTask = Services.GetService<LoadCinemaCityCinemasTask>();

            var getMoviesTask = Services.GetService<LoadCinemaCityMoviesTask>();

            await getCinemasTask.ExecuteAsync();
            await getMoviesTask.ExecuteAsync();

            var showtimesCount = context.MovieShowtimes.Count();

            // Act

            await TaskUnderTest.ExecuteAsync();

            // Assert

            var newShowtimesCount = context.MovieShowtimes.Count();

            newShowtimesCount.Should().BeGreaterThan(showtimesCount);
        }

        [Fact]
        public async Task Should_save_all_showtimes_two_times_in_row()
        {
            // Arrange
            var context = GetDbContext<ApplicationContext>();
            var showtimesCount = context.MovieShowtimes.Count();

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

            var getCinemasTask = Services.GetService<LoadCinemaCityCinemasTask>();

            var getMoviesTask = Services.GetService<LoadCinemaCityMoviesTask>();

            await getCinemasTask.ExecuteAsync();
            await getMoviesTask.ExecuteAsync();

            await TaskUnderTest.ExecuteAsync();

            var countAfterOneExecution = context.MovieShowtimes.Count();

            var newCinemaCityShowtimesTaskInstance = Services.GetService<LoadCinemaCityShowtimesTask>();

            await newCinemaCityShowtimesTaskInstance.ExecuteAsync();

            // Assert

            var newShowtimesCount = context.MovieShowtimes.Count();

            newShowtimesCount.Should().BeGreaterThan(showtimesCount);
            newShowtimesCount.Should().Be(countAfterOneExecution);
        }
    }
}