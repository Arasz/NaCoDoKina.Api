using FluentAssertions;
using Moq;
using NaCoDoKina.Api.Exceptions;
using NaCoDoKina.Api.Models;
using NaCoDoKina.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Services
{
    public class CinemaServiceTest : ServiceWithRepositoryTestBase<ICinemaService, ICinemaRepository>
    {
        protected Mock<ITravelService> TravelServiceMock { get; }

        protected ITravelService TravelServiceFake { get; }

        public CinemaServiceTest()
        {
            TravelServiceFake = new TravelServiceFakeImpl();
            TravelServiceMock = new Mock<ITravelService>();

            ServiceUnderTest = new CinemaService(RepositoryMockObject, TravelServiceFake,
                LoggerMock.Object, MapperMock.Object);
        }

        private class TravelServiceFakeImpl : ITravelService
        {
            public Task<TravelInformation> CalculateInformationForTravelAsync(TravelPlan travelPlan)
            {
                var distance = CalculateEuclideanDistance(travelPlan);

                return Task.FromResult(new TravelInformation(travelPlan, distance, TimeSpan.FromSeconds(1000)));
            }

            private static double CalculateEuclideanDistance(TravelPlan travelPlan)
            {
                var distance = Math.Pow((travelPlan.Origin.Latitude - travelPlan.Destination.Latitude), 2)
                               + Math.Pow((travelPlan.Origin.Longitude - travelPlan.Destination.Longitude), 2);
                return Math.Sqrt(distance);
            }

            public Task<Location> TranslateAddressToLocationAsync(string address)
            {
                throw new NotImplementedException();
            }
        }

        public class GetNearestCinemasForMovieAsync : CinemaServiceTest
        {
            [Fact]
            public async Task Should_return_nearest_cinemas_for_correct_parameters()
            {
                //Arrange
                var location = new Location(1, 2);
                var searchArea = new SearchArea(location, 1000);
                var movieId = 69;
                var cinemas = new List<Entities.Cinema>
                {
                    new Entities.Cinema
                    {
                        Name = "NearCinema",
                        Location = new Entities.Location(15,32)
                    },
                    new Entities.Cinema
                    {
                        Name = "FarCinema",
                        Location = new Entities.Location(1333,4322)
                    }
                };

                MapperMock
                    .Setup(mapper => mapper.Map<Cinema>(It.IsAny<Entities.Cinema>()))
                    .Returns(new Func<Entities.Cinema, Cinema>(cinema => new Cinema
                    {
                        Name = cinema.Name,
                        Location = new Location(cinema.Location.Longitude, cinema.Location.Latitude)
                    }));

                RepositoryMock
                    .Setup(repository => repository.GetAllCinemasForMovie(movieId))
                    .Returns(() => Task.FromResult(cinemas.AsEnumerable()));

                //Act
                var result = await ServiceUnderTest.GetNearestCinemasForMovieAsync(movieId, searchArea);

                //Assert
                result.Should().HaveCount(cinemas.Count - 1);
                result.SingleOrDefault(cinema => cinema.Name == "NearCinema")
                    .Should()
                    .NotBeNull();
            }

            [Fact]
            public void Should_throw_CinemasNotFound_when_could_not_find_movie()
            {
                //Arrange
                var location = new Location(1, 2);
                var searchArea = new SearchArea(location, 100);
                var movieId = -500;

                //Act
                Func<Task<IEnumerable<Cinema>>> action = () => ServiceUnderTest.GetNearestCinemasForMovieAsync(movieId, searchArea);

                //Assert
                action.ShouldThrow<CinemasNotFoundException>();
            }

            [Fact]
            public void Should_throw_CinemasNotFound_when_could_not_find_cinema_near_location()
            {
                //Arrange
                var location = new Location(33, 33);
                var searchArea = new SearchArea(location, 100);
                var movieId = 69;

                //Act
                Func<Task<IEnumerable<Cinema>>> action = () => ServiceUnderTest.GetNearestCinemasForMovieAsync(movieId, searchArea);

                //Assert
                action.ShouldThrow<CinemasNotFoundException>();
            }
        }
    }
}