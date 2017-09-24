using FluentAssertions;
using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Repository
{
    public class CinemaRepositoryTest : RepositoryTestBase<ICinemaRepository>
    {
        public class GetAllCinemasForMovie : RepositoryTestBase<ICinemaRepository>
        {
            [Fact]
            public async Task Should_return_all_cinemas_for_movie()
            {
                using (var databaseScope = new InMemoryDatabaseScope())
                {
                    //Arrange
                    EnsureCreated(databaseScope);
                    var cinema1 = new Cinema
                    {
                        Address = nameof(Cinema.Address),
                        Location = new Location(1, 1),
                        Name = $"{nameof(Cinema)}1",
                    };
                    var cinema2 = new Cinema
                    {
                        Address = nameof(Cinema.Address),
                        Location = new Location(1, 2),
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
                        Name = $"{nameof(Movie)}1",
                        Details = new MovieDetails
                        {
                            Description = nameof(MovieDetails),
                        },
                    };
                    var movie2 = new Movie
                    {
                        Name = $"{nameof(Movie)}2",
                        Details = new MovieDetails
                        {
                            Description = nameof(MovieDetails),
                        },
                    };

                    var movies = new List<Movie> { movie1, movie2 };

                    var movieShowtimes = new List<MovieShowtime>
                    {
                        new MovieShowtime
                        {
                            Cinema = cinema1,
                            ShowTime = DateTime.Now.AddHours(1),
                            Movie = movie1,
                        },
                        new MovieShowtime
                        {
                            Cinema = cinema1,
                            ShowTime = DateTime.Now,
                            Movie = movie1,
                        },
                        new MovieShowtime
                        {
                            Cinema = cinema2,
                            Movie = movie2,
                            ShowTime = DateTime.Now.AddHours(1),
                        }
                    };

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Cinemas.AddRange(cinemas);
                        contextScope.ApplicationContext.MovieShowtimes.AddRange(movieShowtimes);
                        await contextScope.ApplicationContext.SaveChangesAsync();

                        foreach (var movie in contextScope.ApplicationContext.Movies)
                        {
                            movies.Single(m => m.Name == movie.Name).Id = movie.Id;
                        }
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        RepositoryUnderTest = new CinemaRepository(contextScope.ApplicationContext, LoggerMock.Object);

                        //Act
                        var returnedCinemas = await RepositoryUnderTest.GetAllCinemasForMovieAsync(movie1.Id);

                        //Assert
                        returnedCinemas.Should().HaveCount(1);
                        returnedCinemas.Single().Name.Should().Be(cinema1.Name);
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Cinemas.Should().HaveSameCount(cinemas);
                        contextScope.ApplicationContext.Movies.Should().HaveSameCount(movies);
                    }
                }
            }
        }

        public class AddCinema : CinemaRepositoryTest
        {
            [Fact]
            public async Task Should_add_cinema_to_database()
            {
                using (var databaseScope = new InMemoryDatabaseScope())
                {
                    //Arrange
                    EnsureCreated(databaseScope);
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

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        RepositoryUnderTest = new CinemaRepository(contextScope.ApplicationContext, LoggerMock.Object);

                        //Act
                        var returnedCinema = await RepositoryUnderTest.AddCinema(cinema);

                        //Assert
                        returnedCinema.Id.Should().BeGreaterThan(0);
                        returnedCinema.CinemaNetwork.Id.Should().BeGreaterThan(0);
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Cinemas.Count().Should().Be(1);
                        contextScope.ApplicationContext.CinemaNetworks.Count().Should().Be(1);
                    }
                }
            }

            [Fact]
            public async Task Should_throw_when_adding_cinema_with_duplicate_name()
            {
                using (var databaseScope = new InMemoryDatabaseScope())
                {
                    //Arrange
                    EnsureCreated(databaseScope);
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

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Cinemas.Add(cinema1);
                        await contextScope.ApplicationContext.SaveChangesAsync();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        RepositoryUnderTest = new CinemaRepository(contextScope.ApplicationContext, LoggerMock.Object);

                        //Act
                        Func<Task<Cinema>> action = () => RepositoryUnderTest.AddCinema(cinema);

                        //Assert
                        action.ShouldThrow<Exception>();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Cinemas.Count().Should().Be(1);
                        contextScope.ApplicationContext.CinemaNetworks.Count().Should().Be(1);
                    }
                }
            }
        }

        public class GetAllCinemas : CinemaRepositoryTest
        {
            [Fact]
            public async Task Should_return_all_cinemas()
            {
                using (var databaseScope = new InMemoryDatabaseScope())
                {
                    //Arrange
                    EnsureCreated(databaseScope);
                    var cinema1 = new Cinema
                    {
                        Address = nameof(Cinema.Address),
                        Location = new Location(1, 1),
                        Name = $"{nameof(Cinema)}1",
                    };
                    var cinema2 = new Cinema
                    {
                        Address = nameof(Cinema.Address),
                        Location = new Location(1, 2),
                        Name = $"{nameof(Cinema)}2",
                    };

                    var cinemas = new[] { cinema1, cinema2 };

                    var cinemaNetwork = new CinemaNetwork
                    {
                        Name = nameof(CinemaNetwork),
                    };

                    foreach (var cinema in cinemas)
                        cinema.CinemaNetwork = cinemaNetwork;

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Cinemas.AddRange(cinemas);
                        await contextScope.ApplicationContext.SaveChangesAsync();
                    }

                    IEnumerable<Cinema> returnedCinemas;
                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        RepositoryUnderTest = new CinemaRepository(contextScope.ApplicationContext, LoggerMock.Object);

                        //Act
                        returnedCinemas = await RepositoryUnderTest.GetAllCinemas();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        //Assert
                        returnedCinemas.Should()
                            .Match(enumerable => enumerable.All(cinema => cinema.Id > 0));

                        // Needs equals operators implementation
                        //contextScope.ApplicationContext.Cinemas
                        //    .Include(c => c.CinemaNetwork)
                        //    .Should().BeEquivalentTo(returnedCinemas);

                        contextScope.ApplicationContext.Cinemas.Should().HaveCount(cinemas.Length);
                        contextScope.ApplicationContext.CinemaNetworks.Should().HaveCount(1);
                    }
                }
            }
        }

        public class GetCinemaAsync : CinemaRepositoryTest
        {
            [Fact]
            public async Task Should_return_cinema_with_given_id()
            {
                using (var databaseScope = new InMemoryDatabaseScope())
                {
                    //Arrange
                    EnsureCreated(databaseScope);

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
                        Location = new Location(1, 2),
                        Name = $"{nameof(Cinema)}2",
                    };

                    var cinemas = new[] { cinema1, cinema2 };

                    var cinemaNetwork = new CinemaNetwork
                    {
                        Name = nameof(CinemaNetwork),
                    };

                    foreach (var cinema in cinemas)
                        cinema.CinemaNetwork = cinemaNetwork;

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Cinemas.AddRange(cinemas);
                        await contextScope.ApplicationContext.SaveChangesAsync();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        RepositoryUnderTest = new CinemaRepository(contextScope.ApplicationContext, LoggerMock.Object);

                        //Act
                        var returnedCinema = await RepositoryUnderTest.GetCinemaAsync(cinema1Id);

                        returnedCinema.Should().NotBeNull();
                        returnedCinema.Name.Should().Be(cinema1.Name);
                        returnedCinema.Id.Should().Be(cinema1Id);
                    }
                }
            }

            [Fact]
            public async Task Should_return_cinema_with_given_name()
            {
                using (var databaseScope = new InMemoryDatabaseScope())
                {
                    //Arrange
                    EnsureCreated(databaseScope);

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
                        Location = new Location(1, 2),
                        Name = $"{nameof(Cinema)}2",
                    };

                    var cinemas = new[] { cinema1, cinema2 };

                    var cinemaNetwork = new CinemaNetwork
                    {
                        Name = nameof(CinemaNetwork),
                    };

                    foreach (var cinema in cinemas)
                        cinema.CinemaNetwork = cinemaNetwork;

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Cinemas.AddRange(cinemas);
                        await contextScope.ApplicationContext.SaveChangesAsync();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        RepositoryUnderTest = new CinemaRepository(contextScope.ApplicationContext, LoggerMock.Object);

                        //Act
                        var returnedCinema = await RepositoryUnderTest.GetCinemaAsync(cinema1.Name);

                        returnedCinema.Should().NotBeNull();
                        returnedCinema.Name.Should().Be(cinema1.Name);
                        returnedCinema.Id.Should().Be(cinema1Id);
                    }
                }
            }

            [Fact]
            public async Task Should_null_when_cant_find_cinema()
            {
                using (var databaseScope = new InMemoryDatabaseScope())
                {
                    //Arrange
                    EnsureCreated(databaseScope);

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
                        Location = new Location(1, 2),
                        Name = $"{nameof(Cinema)}2",
                    };

                    var cinemas = new[] { cinema1, cinema2 };

                    var cinemaNetwork = new CinemaNetwork
                    {
                        Name = nameof(CinemaNetwork),
                    };

                    foreach (var cinema in cinemas)
                        cinema.CinemaNetwork = cinemaNetwork;

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Cinemas.AddRange(cinemas);
                        await contextScope.ApplicationContext.SaveChangesAsync();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        RepositoryUnderTest = new CinemaRepository(contextScope.ApplicationContext, LoggerMock.Object);

                        //Act
                        var returnedCinema = await RepositoryUnderTest.GetCinemaAsync(nonExistingId);

                        returnedCinema.Should().BeNull();
                    }
                }
            }
        }
    }
}