using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.DataProviders.CinemaCity.Cinemas;
using NaCoDoKina.Api.Entities.Resources;
using NaCoDoKina.Api.Infrastructure.Settings;
using NaCoDoKina.Api.IntegrationTests.Api;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using CinemaNetwork = NaCoDoKina.Api.Entities.Cinemas.CinemaNetwork;

namespace NaCoDoKina.Api.IntegrationTests.DataProviders
{
    public class CinemaCityCinemasTaskTest : HttpTestWithDatabase
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
                Url = new ResourceLink(networksSettings.CinemaCityNetwork.Url),
            };

            context.CinemaNetworks.Add(cinemaCityNetwork);

            await context.SaveChangesAsync();

            var cinemasCount = context.Cinemas.Count();

            var task = Services.GetService<CinemaCityCinemasTask>();

            // Act

            await task.Execute();

            // Assert

            var newCount = context.Cinemas.Count();

            newCount.Should().BeGreaterThan(cinemasCount);

            //var id = BackgroundJob.Enqueue(() => task.Execute());
        }
    }
}