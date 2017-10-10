﻿using FluentAssertions;
using Infrastructure.Data;
using Infrastructure.DataProviders.CinemaCity.Cinemas.Tasks;
using Infrastructure.DataProviders.CinemaCity.Movies.Tasks;
using Infrastructure.DataProviders.CinemaCity.Showtimes.Tasks;
using Infrastructure.Settings;
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