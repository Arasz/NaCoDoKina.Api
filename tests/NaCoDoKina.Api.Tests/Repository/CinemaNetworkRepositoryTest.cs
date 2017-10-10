using ApplicationCore.Entities.Cinemas;
using FluentAssertions;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Ploeh.AutoFixture;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Repository
{
    public class CinemaNetworkRepositoryTest : ApplicationRepositoryTestBase<CinemaNetworkRepository>
    {
        public class ExistAsync : CinemaNetworkRepositoryTest
        {
            [Fact]
            public async Task Should_return_true_when_network_with_given_name_exist()
            {
                // Arrange
                var cinemaNetwork = Fixture.Build<CinemaNetwork>()
                    .Without(network => network.Id)
                    .Create();

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Add(cinemaNetwork);
                    await scope.DbContext.SaveChangesAsync();
                }

                using (var scope = CreateContextScope())
                {
                    RepositoryUnderTest = new CinemaNetworkRepository(scope.DbContext);

                    // Act
                    var exist = await RepositoryUnderTest.ExistAsync(cinemaNetwork.Name);

                    // Assert
                    exist.Should().BeTrue();
                }
            }

            [Fact]
            public async Task Should_return_false_when_network_with_given_name_do_not_exist()
            {
                // Arrange

                var nonExistingName = Fixture.Create<string>();

                var cinemaNetwork = Fixture.Build<CinemaNetwork>()
                    .Without(network => network.Id)
                    .Create();

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Add(cinemaNetwork);
                    await scope.DbContext.SaveChangesAsync();
                }

                using (var scope = CreateContextScope())
                {
                    RepositoryUnderTest = new CinemaNetworkRepository(scope.DbContext);

                    // Act
                    var exist = await RepositoryUnderTest.ExistAsync(nonExistingName);

                    // Assert
                    exist.Should().BeFalse();
                }
            }
        }

        public class CreateAsync : CinemaNetworkRepositoryTest
        {
            [Fact]
            public async Task Should_create_new_network_and_return_id()
            {
                // Arrange

                var cinemaNetwork = Fixture.Build<CinemaNetwork>()
                    .Without(network => network.Id)
                    .Create();

                using (var scope = CreateContextScope())
                {
                    RepositoryUnderTest = new CinemaNetworkRepository(scope.DbContext);

                    // Act
                    var id = await RepositoryUnderTest.CreateAsync(cinemaNetwork);

                    // Assert
                    id.Should().BePositive();
                }

                using (var scope = CreateContextScope())
                {
                    var createdNetwork = await scope.DbContext.CinemaNetworks
                        .SingleOrDefaultAsync(network => network.Name == cinemaNetwork.Name);

                    createdNetwork.Should()
                        .NotBeNull();

                    createdNetwork.CinemaNetworkUrl.Should()
                        .Be(cinemaNetwork.CinemaNetworkUrl);
                }
            }
        }

        public class GetByIdAsync : CinemaNetworkRepositoryTest
        {
            [Fact]
            public async Task Should_return_cinema_network_with_given_id_when_exist()
            {
                // Arrange
                var cinemaNetwork = Fixture.Build<CinemaNetwork>()
                    .Without(network => network.Id)
                    .Create();

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Add(cinemaNetwork);
                    await scope.DbContext.SaveChangesAsync();
                }

                using (var scope = CreateContextScope())
                {
                    RepositoryUnderTest = new CinemaNetworkRepository(scope.DbContext);

                    // Act
                    var existingCinemaNetwork = await RepositoryUnderTest.GetByIdAsync(cinemaNetwork.Id);

                    // Assert
                    existingCinemaNetwork.Should().NotBeNull();
                    existingCinemaNetwork.Name.Should().Be(cinemaNetwork.Name);
                    existingCinemaNetwork.Id.Should().Be(existingCinemaNetwork.Id);
                }
            }

            [Fact]
            public async Task Should_return_null_when_cinema_network_can_not_be_found()
            {
                // Arrange
                var cinemaNetwork = Fixture.Build<CinemaNetwork>()
                    .Without(network => network.Id)
                    .Create();

                var nonExistingId = Fixture.Create<long>();

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Add(cinemaNetwork);
                    await scope.DbContext.SaveChangesAsync();
                }

                using (var scope = CreateContextScope())
                {
                    RepositoryUnderTest = new CinemaNetworkRepository(scope.DbContext);

                    // Act
                    var existingCinemaNetwork = await RepositoryUnderTest.GetByIdAsync(nonExistingId);

                    // Assert
                    existingCinemaNetwork.Should().BeNull();
                }
            }
        }

        public class GetByNameAsync : CinemaNetworkRepositoryTest
        {
            [Fact]
            public async Task Should_return_cinema_network_with_given_id_when_exist()
            {
                // Arrange
                var cinemaNetwork = Fixture.Build<CinemaNetwork>()
                    .Without(network => network.Id)
                    .Create();

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Add(cinemaNetwork);
                    await scope.DbContext.SaveChangesAsync();
                }

                using (var scope = CreateContextScope())
                {
                    RepositoryUnderTest = new CinemaNetworkRepository(scope.DbContext);

                    // Act
                    var existingCinemaNetwork = await RepositoryUnderTest.GetByNameAsync(cinemaNetwork.Name);

                    // Assert
                    existingCinemaNetwork.Should().NotBeNull();
                    existingCinemaNetwork.Name.Should().Be(cinemaNetwork.Name);
                    existingCinemaNetwork.Id.Should().Be(existingCinemaNetwork.Id);
                }
            }

            [Fact]
            public async Task Should_return_null_when_cinema_network_can_not_be_found()
            {
                // Arrange
                var cinemaNetwork = Fixture.Build<CinemaNetwork>()
                    .Without(network => network.Id)
                    .Create();

                var nonExistingName = Fixture.Create<string>();

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Add(cinemaNetwork);
                    await scope.DbContext.SaveChangesAsync();
                }

                using (var scope = CreateContextScope())
                {
                    RepositoryUnderTest = new CinemaNetworkRepository(scope.DbContext);

                    // Act
                    var existingCinemaNetwork = await RepositoryUnderTest.GetByNameAsync(nonExistingName);

                    // Assert
                    existingCinemaNetwork.Should().BeNull();
                }
            }
        }
    }
}