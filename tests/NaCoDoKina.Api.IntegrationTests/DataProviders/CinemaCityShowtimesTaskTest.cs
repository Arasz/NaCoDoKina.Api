using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.DataProviders.CinemaCity.Cinemas.Tasks;
using NaCoDoKina.Api.DataProviders.CinemaCity.Showtimes.Tasks;
using NaCoDoKina.Api.Infrastructure.Settings;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using CinemaNetwork = NaCoDoKina.Api.Entities.Cinemas.CinemaNetwork;

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

            await getCinemasTask.Execute();

            var showtimesCount = context.MovieShowtimes.Count();

            // Act

            await TaskUnderTest.Execute();

            // Assert

            var newShowtimesCount = context.MovieShowtimes.Count();

            newShowtimesCount.Should().BeGreaterThan(showtimesCount);
        }
    }
}