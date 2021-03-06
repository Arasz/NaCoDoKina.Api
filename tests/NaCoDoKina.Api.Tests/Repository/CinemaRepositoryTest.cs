﻿using ApplicationCore.Entities;
using ApplicationCore.Entities.Cinemas;
using ApplicationCore.Entities.Movies;
using FluentAssertions;
using Infrastructure.Repositories;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Repository
{
    public class CinemaRepositoryTest : ApplicationRepositoryTestBase<CinemaRepository>
    {
        public class GetAllCinemasForMovie : CinemaRepositoryTest
        {
            [Fact]
            public async Task Should_return_all_cinemas_for_movie()
            {
                //Arrange
                var cinema1 = new Cinema
                {
                    Address = nameof(Cinema.Address),
                    Location = new Location(1, 1),
                    Name = $"{nameof(Cinema)}1",
                };
                var cinema2 = new Cinema
                {
                    Address = nameof(Cinema.Address),
                    Location = new Location(2, 1),
                    Name = $"{nameof(Cinema)}2",
                };

                var cinemas = new[] { cinema1, cinema2 };

                var cinemaNetwork = new CinemaNetwork
                {
                    Name = nameof(CinemaNetwork),
                };

                foreach (var cinema in cinemas)
                    cinema.CinemaNetwork = cinemaNetwork;

                var movie1 = new Movie
                {
                    Title = $"{nameof(Movie)}1",
                    Details = Fixture.Build<MovieDetails>()
                        .Without(details => details.Id)
                        .Create(),
                    PosterUrl = Fixture.Create<string>()
                };
                var movie2 = new Movie
                {
                    Title = $"{nameof(Movie)}2",
                    Details = Fixture.Build<MovieDetails>()
                        .Without(details => details.Id)
                        .Create(),
                    PosterUrl = Fixture.Create<string>()
                };

                var movies = new List<Movie> { movie1, movie2 };

                var movieShowtimes = new List<MovieShowtime>
                    {
                        new MovieShowtime
                        {
                            Cinema = cinema1,
                            ShowTime = DateTime.Now.AddHours(1),
                            Movie = movie1,
                            Language = "",
                            ShowType = ""
                        },
                        new MovieShowtime
                        {
                            Cinema = cinema1,
                            ShowTime = DateTime.Now,
                            Movie = movie1,
                            Language = "",
                            ShowType = ""
                        },
                        new MovieShowtime
                        {
                            Cinema = cinema2,
                            Movie = movie2,
                            ShowTime = DateTime.Now.AddHours(1),
                            Language = "",
                            ShowType = ""
                        }
                    };

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.Cinemas.AddRange(cinemas);
                    contextScope.DbContext.MovieShowtimes.AddRange(movieShowtimes);
                    await contextScope.DbContext.SaveChangesAsync();

                    foreach (var movie in contextScope.DbContext.Movies)
                    {
                        movies.Single(m => m.Title == movie.Title).Id = movie.Id;
                    }
                }

                using (CreateContextScope())
                {
                    //Act
                    var returnedCinemas = await RepositoryUnderTest.GetAllCinemasForMovieAsync(movie1.Id);

                    //Assert
                    returnedCinemas.Should().HaveCount(1);
                    returnedCinemas.Single().Name.Should().Be(cinema1.Name);
                }

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.Cinemas.Should().HaveSameCount(cinemas);
                    contextScope.DbContext.Movies.Should().HaveSameCount(movies);
                }
            }
        }

        public class CreateCinemasAsync : CinemaRepositoryTest
        {
            [Fact]
            public async Task Should_add_cinemas_to_database()
            {
                //Arrange
                var cinemasCount = 5;
                var cinemaNetwork = Fixture.Build<CinemaNetwork>()
                    .Without(network => network.Id)
                    .Create();

                var cinemas = Fixture.Build<Cinema>()
                    .Without(cinema => cinema.Id)
                    .With(cinema => cinema.CinemaNetwork, cinemaNetwork)
                    .CreateMany(cinemasCount);

                using (CreateContextScope())
                {
                    //Act
                    await RepositoryUnderTest.CreateCinemasAsync(cinemas);
                }

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.Cinemas.Count().Should().Be(cinemasCount);
                }
            }
        }

        public class CreateCinemaAsync : CinemaRepositoryTest
        {
            [Fact]
            public async Task Should_add_cinema_to_database()
            {
                //Arrange

                var location = new Location(1, 1);
                var cinema = new Cinema
                {
                    Address = nameof(Cinema.Address),
                    Location = location,
                    Name = nameof(Cinema),
                };

                var cinemaNetwork = new CinemaNetwork
                {
                    Name = nameof(CinemaNetwork),
                };

                cinema.CinemaNetwork = cinemaNetwork;

                using (CreateContextScope())
                {
                    //Act
                    var returnedCinema = await RepositoryUnderTest.CreateCinemaAsync(cinema);

                    //Assert
                    returnedCinema.Id.Should().BeGreaterThan(0);
                    returnedCinema.CinemaNetwork.Id.Should().BeGreaterThan(0);
                }

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.Cinemas.Count().Should().Be(1);
                    contextScope.DbContext.CinemaNetworks.Count().Should().Be(1);
                }
            }

            [Fact]
            public async Task Should_throw_when_adding_cinema_with_duplicate_name()
            {
                //Arrange

                var location = new Location(1, 1);
                var cinema = new Cinema
                {
                    Address = nameof(Cinema.Address),
                    Location = location,
                    Name = nameof(Cinema),
                };

                var cinema1 = new Cinema
                {
                    Address = nameof(Cinema.Address),
                    Location = new Location(2, 2),
                    Name = nameof(Cinema),
                };

                var cinemaNetwork = new CinemaNetwork
                {
                    Name = nameof(CinemaNetwork),
                };

                cinema.CinemaNetwork = cinemaNetwork;
                cinema1.CinemaNetwork = cinemaNetwork;

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.Cinemas.Add(cinema1);
                    await contextScope.DbContext.SaveChangesAsync();
                }

                using (CreateContextScope())
                {
                    //Act
                    Func<Task<Cinema>> action = () => RepositoryUnderTest.CreateCinemaAsync(cinema);

                    //Assert
                    action.ShouldThrow<Exception>();
                }

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.Cinemas.Count().Should().Be(1);
                    contextScope.DbContext.CinemaNetworks.Count().Should().Be(1);
                }
            }
        }

        public class GetAllCinemasByCity : CinemaRepositoryTest
        {
            [Fact]
            public async Task Should_return_all_cinemas()
            {
                //Arrange

                IEnumerable<Cinema> BuildCinemas(string city)
                {
                    return Fixture.Build<Cinema>()
                        .With(cinema => cinema.Address, $"ul. Zakopiańska 62, 30-418, {city}")
                        .Without(cinema => cinema.Id)
                        .CreateMany(3);
                }

                var chosenCity = "Kraków";
                var cinemas = BuildCinemas(chosenCity)
                    .ToList();

                cinemas.AddRange(BuildCinemas("Poznań"));

                using (var context = CreateContextScope())
                {
                    context.DbContext.Cinemas.AddRange(cinemas);
                    await context.DbContext.SaveChangesAsync();
                }

                using (CreateContextScope())
                {
                    //Act
                    var returnCinemas = await RepositoryUnderTest.GetCinemasByCityAsync(chosenCity);

                    //Assert
                    returnCinemas
                        .Should()
                        .Match(c => c.All(cinema => cinema.Address.EndsWith(chosenCity)))
                        .And
                        .HaveCount(3);
                }
            }
        }

        public class GetAllCinemas : CinemaRepositoryTest
        {
            [Fact]
            public async Task Should_return_all_cinemas()
            {
                //Arrange

                var cinema1 = new Cinema
                {
                    Address = nameof(Cinema.Address),
                    Location = new Location(1, 1),
                    Name = $"{nameof(Cinema)}1",
                };
                var cinema2 = new Cinema
                {
                    Address = nameof(Cinema.Address),
                    Location = new Location(2, 1),
                    Name = $"{nameof(Cinema)}2",
                };

                var cinemas = new[] { cinema1, cinema2 };

                var cinemaNetwork = new CinemaNetwork
                {
                    Name = nameof(CinemaNetwork),
                };

                foreach (var cinema in cinemas)
                    cinema.CinemaNetwork = cinemaNetwork;

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.Cinemas.AddRange(cinemas);
                    await contextScope.DbContext.SaveChangesAsync();
                }

                IEnumerable<Cinema> returnedCinemas;
                using (CreateContextScope())
                {
                    //Act
                    returnedCinemas = await RepositoryUnderTest.GetAllCinemas();
                }

                using (var contextScope = CreateContextScope())
                {
                    //Assert
                    returnedCinemas.Should()
                        .Match(enumerable => enumerable.All(cinema => cinema.Id > 0));

                    // Needs equals operators implementation
                    //contextScope.DbContext.Cinemas
                    //    .Include(c => c.CinemaNetwork)
                    //    .Should().BeEquivalentTo(returnedCinemas);

                    contextScope.DbContext.Cinemas.Should().HaveCount(cinemas.Length);
                    contextScope.DbContext.CinemaNetworks.Should().HaveCount(1);
                }
            }
        }

        public class GetCinemaAsync : CinemaRepositoryTest
        {
            [Fact]
            public async Task Should_return_cinema_with_given_id()
            {
                //Arrange

                var cinema1Id = 1;

                var cinema1 = new Cinema
                {
                    Address = nameof(Cinema.Address),
                    Location = new Location(1, 1),
                    Name = $"{nameof(Cinema)}1",
                };
                var cinema2 = new Cinema
                {
                    Address = nameof(Cinema.Address),
                    Location = new Location(2, 1),
                    Name = $"{nameof(Cinema)}2",
                };

                var cinemas = new[] { cinema1, cinema2 };

                var cinemaNetwork = new CinemaNetwork
                {
                    Name = nameof(CinemaNetwork),
                };

                foreach (var cinema in cinemas)
                    cinema.CinemaNetwork = cinemaNetwork;

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.Cinemas.AddRange(cinemas);
                    await contextScope.DbContext.SaveChangesAsync();
                }

                using (CreateContextScope())
                {
                    //Act
                    var returnedCinema = await RepositoryUnderTest.GetCinemaByIdAsync(cinema1Id);

                    returnedCinema.Should().NotBeNull();
                    returnedCinema.Name.Should().Be(cinema1.Name);
                    returnedCinema.Id.Should().Be(cinema1Id);
                }
            }

            [Fact]
            public async Task Should_return_cinema_with_given_name()
            {
                //Arrange

                var cinema1Id = 1;

                var cinema1 = new Cinema
                {
                    Address = nameof(Cinema.Address),
                    Location = new Location(1, 1),
                    Name = $"{nameof(Cinema)}1",
                };
                var cinema2 = new Cinema
                {
                    Address = nameof(Cinema.Address),
                    Location = new Location(2, 1),
                    Name = $"{nameof(Cinema)}2",
                };

                var cinemas = new[] { cinema1, cinema2 };

                var cinemaNetwork = new CinemaNetwork
                {
                    Name = nameof(CinemaNetwork),
                };

                foreach (var cinema in cinemas)
                    cinema.CinemaNetwork = cinemaNetwork;

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.Cinemas.AddRange(cinemas);
                    await contextScope.DbContext.SaveChangesAsync();
                }

                using (CreateContextScope())
                {
                    //Act
                    var returnedCinema = await RepositoryUnderTest.GetCinemaByNameAsync(cinema1.Name);

                    returnedCinema.Should().NotBeNull();
                    returnedCinema.Name.Should().Be(cinema1.Name);
                    returnedCinema.Id.Should().Be(cinema1Id);
                }
            }

            [Fact]
            public async Task Should_null_when_cant_find_cinema()
            {
                //Arrange

                var nonExistingId = 22;

                var cinema1 = new Cinema
                {
                    Address = nameof(Cinema.Address),
                    Location = new Location(1, 1),
                    Name = $"{nameof(Cinema)}1",
                };
                var cinema2 = new Cinema
                {
                    Address = nameof(Cinema.Address),
                    Location = new Location(2, 1),
                    Name = $"{nameof(Cinema)}2",
                };

                var cinemas = new[] { cinema1, cinema2 };

                var cinemaNetwork = new CinemaNetwork
                {
                    Name = nameof(CinemaNetwork),
                };

                foreach (var cinema in cinemas)
                    cinema.CinemaNetwork = cinemaNetwork;

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.Cinemas.AddRange(cinemas);
                    await contextScope.DbContext.SaveChangesAsync();
                }

                using (CreateContextScope())
                {
                    //Act
                    var returnedCinema = await RepositoryUnderTest.GetCinemaByIdAsync(nonExistingId);

                    returnedCinema.Should().BeNull();
                }
            }
        }
    }
}