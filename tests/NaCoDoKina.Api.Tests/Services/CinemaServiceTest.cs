using ApplicationCore.Repositories;
using FluentAssertions;
using Infrastructure.Exceptions;
using Infrastructure.Models;
using Infrastructure.Models.Cinemas;
using Infrastructure.Models.Travel;
using Infrastructure.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Services
{
    public class CinemaServiceTest : ServiceWithRepositoryTestBase<CinemaService, ICinemaRepository>
    {
        protected ITravelService TravelServiceFake { get; }

        public CinemaServiceTest()
        {
            TravelServiceFake = new TravelServiceFakeImpl();
            Mock.Provide(TravelServiceFake);
        }

        private class TravelServiceFakeImpl : ITravelService
        {
            public Task<TravelInformation> GetInformationForTravelAsync(TravelPlan travelPlan)
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

        public class AddCinemaAsync : CinemaServiceTest
        {
            [Fact]
            public async Task Should_return_added_cinema()
            {
                //Arrange
                var modelCinema = new Cinema
                {
                    Id = 0,
                    Name = "A",
                    Address = "Address",
                };
                var entityCinema = new ApplicationCore.Entities.Cinemas.Cinema
                {
                    Id = modelCinema.Id,
                    Name = modelCinema.Name,
                    Address = modelCinema.Address
                };

                MapperMock
                    .Setup(mapper => mapper.Map<ApplicationCore.Entities.Cinemas.Cinema>(modelCinema))
                    .Returns(entityCinema);

                MapperMock
                    .Setup(mapper => mapper.Map<Cinema>(entityCinema))
                    .Returns(new Func<ApplicationCore.Entities.Cinemas.Cinema, Cinema>(cinema => new Cinema
                    {
                        Id = cinema.Id,
                        Name = cinema.Name,
                        Address = cinema.Address
                    }));

                RepositoryMock
                    .Setup(repository => repository.CreateCinemaAsync(entityCinema))
                    .Returns(() =>
                    {
                        entityCinema.Id++;
                        return Task.FromResult(entityCinema);
                    });

                //Act
                var addedCinema = await ServiceUnderTest.AddCinemaAsync(modelCinema);

                //Assert
                addedCinema.Name.Should().Be(entityCinema.Name);
                addedCinema.Address.Should().Be(entityCinema.Address);
                addedCinema.Id.Should().BeGreaterThan(0);
            }

            public class GetCinemasPlayingMovieInSearchAreaAsync : CinemaServiceTest
            {
                [Fact]
                public async Task Should_return_nearest_cinemas_for_correct_parameters()
                {
                    //Arrange
                    var location = new Location(1, 2);
                    var searchArea = new SearchArea(location, 1000);
                    var movieId = 69;
                    var cinemas = new[]
                    {
                        new ApplicationCore.Entities.Cinemas.Cinema
                        {
                            Name = "NearCinema",
                            Location = new ApplicationCore.Entities.Location(15, 32)
                        },
                        new ApplicationCore.Entities.Cinemas.Cinema
                        {
                            Name = "FarCinema",
                            Location = new ApplicationCore.Entities.Location(1333, 4322)
                        }
                    };

                    MapperMock
                        .Setup(mapper => mapper.Map<Cinema>(It.IsAny<ApplicationCore.Entities.Cinemas.Cinema>()))
                        .Returns(new Func<ApplicationCore.Entities.Cinemas.Cinema, Cinema>(cinema => new Cinema
                        {
                            Name = cinema.Name,
                            Location = new Location(cinema.Location.Longitude, cinema.Location.Latitude)
                        }));

                    RepositoryMock
                        .Setup(repository => repository.GetAllCinemasForMovieAsync(movieId))
                        .ReturnsAsync(cinemas);

                    //Act
                    var result = await ServiceUnderTest.GetCinemasPlayingMovieInSearchArea(movieId, searchArea);

                    //Assert
                    result.Should().HaveCount(cinemas.Length - 1);
                    result.SingleOrDefault(cinema => cinema.Name == "NearCinema")
                        .Should()
                        .NotBeNull();
                    result.Should()
                        .Match(enumerable => enumerable.All(cinema => cinema.CinemaTravelInformation != null));
                }

                [Fact]
                public void Should_throw_CinemasNotFound_when_could_not_find_movie()
                {
                    //Arrange
                    var location = new Location(1, 2);
                    var searchArea = new SearchArea(location, 100);
                    var movieId = -500;

                    RepositoryMock
                        .Setup(repository => repository.GetAllCinemasForMovieAsync(movieId))
                        .ReturnsAsync(Array.Empty<ApplicationCore.Entities.Cinemas.Cinema>());

                    //Act
                    Func<Task<ICollection<Cinema>>> action = () =>
                        ServiceUnderTest.GetCinemasPlayingMovieInSearchArea(movieId, searchArea);

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
                    Func<Task<ICollection<Cinema>>> action = () =>
                        ServiceUnderTest.GetCinemasPlayingMovieInSearchArea(movieId, searchArea);

                    //Assert
                    action.ShouldThrow<CinemasNotFoundException>();
                }
            }

            public class GetCinemasInSearchAreaAsync : CinemaServiceTest
            {
                [Fact]
                public async Task Should_return_cinemas_in_search_are_inside_given_city()
                {
                    //Arrange
                    var location = new Location(1, 2);
                    var city = "Poznań";
                    var searchArea = new SearchArea(location, 1000)
                    {
                        City = city
                    };

                    var cinemas = new List<ApplicationCore.Entities.Cinemas.Cinema>
                    {
                        new ApplicationCore.Entities.Cinemas.Cinema
                        {
                            Name = "NearCinema",
                            Location = new ApplicationCore.Entities.Location(15, 32),
                            Address = $"address {city}"
                        },
                        new ApplicationCore.Entities.Cinemas.Cinema
                        {
                            Name = "NearCinema2",
                            Location = new ApplicationCore.Entities.Location(15, 32),
                            Address = $"address Kraków"
                        },
                        new ApplicationCore.Entities.Cinemas.Cinema
                        {
                            Name = "FarCinema",
                            Location = new ApplicationCore.Entities.Location(1333, 4322),
                            Address = $"address {city}"
                        }
                    };

                    MapperMock
                        .Setup(mapper => mapper.Map<Cinema>(It.IsAny<ApplicationCore.Entities.Cinemas.Cinema>()))
                        .Returns(new Func<ApplicationCore.Entities.Cinemas.Cinema, Cinema>(cinema => new Cinema
                        {
                            Name = cinema.Name,
                            Location = new Location(cinema.Location.Longitude, cinema.Location.Latitude)
                        }));

                    RepositoryMock
                        .Setup(repository => repository.GetAllCinemas())
                        .ReturnsAsync(cinemas);

                    //Act
                    var result = await ServiceUnderTest.GetCinemasInSearchAreaAsync(searchArea);

                    //Assert
                    result.Should().HaveCount(cinemas.Count - 2);
                    result.SingleOrDefault(cinema => cinema.Name == "NearCinema")
                        .Should()
                        .NotBeNull();
                }

                [Fact]
                public async Task Should_return_nearest_cinemas_for_correct_parameters()
                {
                    //Arrange
                    var location = new Location(1, 2);
                    var searchArea = new SearchArea(location, 1000);
                    var cinemas = new List<ApplicationCore.Entities.Cinemas.Cinema>
                    {
                        new ApplicationCore.Entities.Cinemas.Cinema
                        {
                            Name = "NearCinema",
                            Location = new ApplicationCore.Entities.Location(15, 32)
                        },
                        new ApplicationCore.Entities.Cinemas.Cinema
                        {
                            Name = "FarCinema",
                            Location = new ApplicationCore.Entities.Location(1333, 4322)
                        }
                    };

                    MapperMock
                        .Setup(mapper => mapper.Map<Cinema>(It.IsAny<ApplicationCore.Entities.Cinemas.Cinema>()))
                        .Returns(new Func<ApplicationCore.Entities.Cinemas.Cinema, Cinema>(cinema => new Cinema
                        {
                            Name = cinema.Name,
                            Location = new Location(cinema.Location.Longitude, cinema.Location.Latitude)
                        }));

                    RepositoryMock
                        .Setup(repository => repository.GetAllCinemas())
                        .ReturnsAsync(cinemas);

                    //Act
                    var result = await ServiceUnderTest.GetCinemasInSearchAreaAsync(searchArea);

                    //Assert
                    result.Should().HaveCount(cinemas.Count - 1);
                    result.SingleOrDefault(cinema => cinema.Name == "NearCinema")
                        .Should()
                        .NotBeNull();
                }

                [Fact]
                public void Should_throw_CinemasNotFound_when_could_not_find_cinema_near_location()
                {
                    //Arrange
                    var location = new Location(33, 33);
                    var searchArea = new SearchArea(location, 100);

                    //Act
                    Func<Task<ICollection<Cinema>>> action = () => ServiceUnderTest.GetCinemasInSearchAreaAsync(searchArea);

                    //Assert
                    action.ShouldThrow<CinemasNotFoundException>();
                }
            }

            public class GetCinemaAsync : CinemaServiceTest
            {
                [Fact]
                public async Task Should_return_cinema_with_given_id()
                {
                    //Arrange
                    var cinemaId = 1;
                    var cinema = new ApplicationCore.Entities.Cinemas.Cinema
                    {
                        Id = cinemaId,
                        Name = "NearCinema",
                        Location = new ApplicationCore.Entities.Location(15, 32)
                    };

                    MapperMock
                        .Setup(mapper => mapper.Map<Cinema>(It.IsAny<ApplicationCore.Entities.Cinemas.Cinema>()))
                        .Returns(new Func<ApplicationCore.Entities.Cinemas.Cinema, Cinema>(c => new Cinema
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Location = new Location(c.Location.Longitude, c.Location.Latitude)
                        }));

                    RepositoryMock
                        .Setup(repository => repository.GetCinemaByIdAsync(cinemaId))
                        .Returns(() => Task.FromResult(cinema));

                    //Act
                    var result = await ServiceUnderTest.GetCinemaAsync(cinemaId);

                    //Assert
                    result.Should().NotBeNull();
                    result.Name.Should().Be(cinema.Name);
                    result.Id.Should().Be(cinemaId);
                }

                [Fact]
                public async Task Should_return_cinema_with_given_name()
                {
                    //Arrange
                    var cinemaId = 1;
                    var cinemaName = "NearCinema";
                    var cinema = new ApplicationCore.Entities.Cinemas.Cinema
                    {
                        Id = cinemaId,
                        Name = cinemaName,
                        Location = new ApplicationCore.Entities.Location(15, 32)
                    };

                    MapperMock
                        .Setup(mapper => mapper.Map<Cinema>(It.IsAny<ApplicationCore.Entities.Cinemas.Cinema>()))
                        .Returns(new Func<ApplicationCore.Entities.Cinemas.Cinema, Cinema>(c => new Cinema
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Location = new Location(c.Location.Longitude, c.Location.Latitude)
                        }));

                    RepositoryMock
                        .Setup(repository => repository.GetCinemaByNameAsync(cinemaName))
                        .Returns(() => Task.FromResult(cinema));

                    //Act
                    var result = await ServiceUnderTest.GetCinemaAsync(cinemaName);

                    //Assert
                    result.Should().NotBeNull();
                    result.Name.Should().Be(cinema.Name);
                    result.Id.Should().Be(cinemaId);
                }

                [Fact]
                public void Should_throw_cinema_not_found_exception_when_cinema_with_id_not_found()
                {
                    //Arrange
                    var cinemaId = 1;
                    var cinemaName = "NearCinema";
                    var nonExistingId = 404;
                    var cinema = new ApplicationCore.Entities.Cinemas.Cinema
                    {
                        Id = cinemaId,
                        Name = cinemaName,
                        Location = new ApplicationCore.Entities.Location(15, 32)
                    };

                    MapperMock
                        .Setup(mapper => mapper.Map<Cinema>(It.IsAny<ApplicationCore.Entities.Cinemas.Cinema>()))
                        .Returns(new Func<ApplicationCore.Entities.Cinemas.Cinema, Cinema>(c => new Cinema
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Location = new Location(c.Location.Longitude, c.Location.Latitude)
                        }));

                    RepositoryMock
                        .Setup(repository => repository.GetCinemaByIdAsync(cinemaId))
                        .Returns(() => Task.FromResult(cinema));

                    //Act
                    Func<Task<Cinema>> action = () => ServiceUnderTest.GetCinemaAsync(nonExistingId);

                    //Assert
                    action.ShouldThrow<CinemaNotFoundException>();
                }

                [Fact]
                public void Should_throw_cinema_not_found_exception_when_cinema_with_name_not_found()
                {
                    //Arrange
                    var cinemaId = 1;
                    var cinemaName = "NearCinema";
                    var nonExistingName = "wololo";
                    var cinema = new ApplicationCore.Entities.Cinemas.Cinema
                    {
                        Id = cinemaId,
                        Name = cinemaName,
                        Location = new ApplicationCore.Entities.Location(15, 32)
                    };

                    MapperMock
                        .Setup(mapper => mapper.Map<Cinema>(It.IsAny<ApplicationCore.Entities.Cinemas.Cinema>()))
                        .Returns(new Func<ApplicationCore.Entities.Cinemas.Cinema, Cinema>(c => new Cinema
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Location = new Location(c.Location.Longitude, c.Location.Latitude)
                        }));

                    RepositoryMock
                        .Setup(repository => repository.GetCinemaByIdAsync(cinemaId))
                        .Returns(() => Task.FromResult(cinema));

                    //Act
                    Func<Task<Cinema>> action = () => ServiceUnderTest.GetCinemaAsync(nonExistingName);

                    //Assert
                    action.ShouldThrow<CinemaNotFoundException>();
                }
            }
        }
    }
}