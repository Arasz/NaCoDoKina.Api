using ApplicationCore.Entities.Cinemas;
using ApplicationCore.Entities.Movies;
using ApplicationCore.Repositories;
using FluentAssertions;
using Infrastructure.DataProviders.CinemaCity.Showtimes.Context;
using Infrastructure.DataProviders.CinemaCity.Showtimes.Tasks;
using Infrastructure.DataProviders.EntityBuilder;
using Infrastructure.DataProviders.Timeline;
using Moq;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TestsCore;
using Xunit;

namespace NaCoDoKina.Api.DataProviders.Tasks
{
    public class LoadCinemaCityShowtimesTaskTest : UnitTestBase
    {
        private Mock<IEntitiesBuilder<MovieShowtime, MovieShowtimesContext>> EntitiesBuilderMock { get; }

        public LoadCinemaCityShowtimesTaskTest()
        {
            EntitiesBuilderMock = Mock.Mock<IEntitiesBuilder<MovieShowtime, MovieShowtimesContext>>();
        }

        public class ExecuteAsync : LoadCinemaCityShowtimesTaskTest
        {
            [Fact]
            public async Task Should_load_all_cinema_city_showtimes()
            {
                // Arrange
                var cinemas = Fixture.Build<Cinema>()
                    .CreateMany(3)
                    .ToArray();

                var createdShowtimes = Fixture.Build<MovieShowtime>()
                    .CreateMany(3)
                    .ToArray();

                var addedShowtimes = new List<MovieShowtime>();

                var testDays = 2;
                var testTimeline = new LimitedTimeline(DateTime.Today, DateTime.Today.AddDays(testDays), TimeSpan.FromDays(1));
                Mock.Provide<ILimitedTimeline>(testTimeline);

                Mock.Mock<ICinemaRepository>()
                    .Setup(r => r.GetAllCinemas())
                    .ReturnsAsync(cinemas);

                Mock.Mock<IMovieShowtimeRepository>()
                    .Setup(r => r.CreateMovieShowtimesAsync(It.IsAny<IEnumerable<MovieShowtime>>()))
                    .Callback<IEnumerable<MovieShowtime>>((args) =>
                    {
                        addedShowtimes.AddRange(args);
                    })
                    .Returns(Task.CompletedTask);

                EntitiesBuilderMock
                    .Setup(builder => builder.BuildMany(It.IsAny<CancellationToken>(), It.IsAny<MovieShowtimesContext>()))
                    .ReturnsAsync(createdShowtimes)
                    .Verifiable();

                EntitiesBuilderMock
                    .SetupGet(builder => builder.Successful)
                    .Returns(true);

                // Act

                var task = Mock.Create<LoadCinemaCityShowtimesTask>();
                await task.ExecuteAsync();

                // Assert

                addedShowtimes
                    .Should()
                    .HaveCount(createdShowtimes.Length * cinemas.Length * (testDays + 1));
            }
        }
    }
}