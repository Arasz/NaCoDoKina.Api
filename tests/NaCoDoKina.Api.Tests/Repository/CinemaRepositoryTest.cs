using FluentAssertions;
using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Repository
{
    public class CinemaRepositoryTest : RepositoryTestBase<ICinemaRepository>
    {
        public CinemaRepositoryTest()
        {
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
                    var cinema = new Cinema()
                    {
                        Address = nameof(Cinema.Address),
                        Location = location,
                        Name = nameof(Cinema),
                    };

                    var cinemaNetwork = new CinemaNetwork
                    {
                        Name = nameof(CinemaNetwork),
                        Cinemas = new[] { cinema },
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
                        Cinemas = cinemas,
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
    }
}