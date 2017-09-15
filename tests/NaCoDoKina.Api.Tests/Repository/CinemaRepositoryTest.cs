using FluentAssertions;
using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Repositories;
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
    }
}