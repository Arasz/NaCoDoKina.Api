using FluentAssertions;
using Moq;
using NaCoDoKina.Api.Repositories;
using Ploeh.AutoFixture;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Services
{
    public class DisabledMovieServiceTest : ServiceTestBase<DisabledMovieService>
    {
        public class DisableMovieForCurrentUserAsync : DisabledMovieServiceTest
        {
            [Fact]
            public async Task Disable_movie_when_user_is_logged_in()
            {
                // Arrange
                var movieId = Fixture.Create<long>();
                var userId = Fixture.Create<long>();

                Mock.Mock<IUserService>()
                    .Setup(service => service.GetCurrentUserId())
                    .Returns(userId);

                Mock.Mock<IDisabledMovieRepository>()
                    .Setup(repository => repository.CreateDisabledMovieAsync(movieId, userId))
                    .Verifiable();

                // Act

                await ServiceUnderTest.DisableMovieForCurrentUserAsync(movieId);

                // Assert
                Mock.Mock<IDisabledMovieRepository>()
                    .Verify(r => r.CreateDisabledMovieAsync(movieId, userId));
            }

            [Fact]
            public async Task Do_nothing_when_no_user_is_logged_in()
            {
                // Arrange
                var movieId = Fixture.Create<long>();
                var userId = Fixture.Create<long>();

                Mock.Mock<IUserService>()
                    .Setup(service => service.GetCurrentUserId())
                    .Returns(0);

                Mock.Mock<IDisabledMovieRepository>()
                    .Setup(repository => repository.CreateDisabledMovieAsync(movieId, userId))
                    .Verifiable();

                // Act

                await ServiceUnderTest.DisableMovieForCurrentUserAsync(movieId);

                // Assert
                Mock.Mock<IDisabledMovieRepository>()
                    .Verify(r => r.CreateDisabledMovieAsync(movieId, userId), Times.Never);
            }
        }

        public class FilterDisabledMoviesForCurrentUserAsync : DisabledMovieServiceTest
        {
            [Fact]
            public async Task Filter_disabled_movies_when_user_is_logged_in()
            {
                // Arrange
                var moviesIds = Fixture.CreateMany<long>(5);
                var userId = Fixture.Create<long>();

                Mock.Mock<IUserService>()
                    .Setup(service => service.GetCurrentUserId())
                    .Returns(0);

                Mock.Mock<IDisabledMovieRepository>()
                    .Setup(repository => repository.GetDisabledMoviesIdsForUserAsync(userId))
                    .ReturnsAsync(moviesIds);

                // Act

                var result = await ServiceUnderTest.FilterDisabledMoviesForCurrentUserAsync(moviesIds);

                // Assert
                result
                    .Should()
                    .BeEmpty();
            }

            [Fact]
            public async Task Return_not_modified_input_when_user_is_not_logged_in()
            {
                // Arrange
                var moviesIds = Fixture.CreateMany<long>(5);
                var userId = Fixture.Create<long>();

                Mock.Mock<IUserService>()
                    .Setup(service => service.GetCurrentUserId())
                    .Returns(0);

                Mock.Mock<IDisabledMovieRepository>()
                    .Setup(repository => repository.GetDisabledMoviesIdsForUserAsync(userId))
                    .Verifiable();

                // Act

                var result = await ServiceUnderTest.FilterDisabledMoviesForCurrentUserAsync(moviesIds);

                // Assert
                result
                    .Should()
                    .BeEquivalentTo(moviesIds);

                Mock.Mock<IDisabledMovieRepository>()
                    .Verify(repository => repository.GetDisabledMoviesIdsForUserAsync(userId), Times.Never);
            }
        }

        public class IsMovieDisabledForGivenUserAsync : DisabledMovieServiceTest
        {
            [Fact]
            public async Task Should_return_true_when_movie_is_disabled_and_user_is_logged_in()
            {
                // Arrange
                var movieId = Fixture.Create<long>();
                var userId = Fixture.Create<long>();

                Mock.Mock<IUserService>()
                    .Setup(service => service.GetCurrentUserId())
                    .Returns(userId);

                Mock.Mock<IDisabledMovieRepository>()
                    .Setup(repository => repository.IsMovieDisabledAsync(movieId, userId))
                    .ReturnsAsync(true);

                // Act

                var result = await ServiceUnderTest.IsMovieDisabledForGivenUserAsync(movieId);

                // Assert
                result
                    .Should()
                    .BeTrue();
            }

            [Fact]
            public async Task Should_return_false_when_movie_is_disabled_and_user_is_logged_in()
            {
                // Arrange
                var movieId = Fixture.Create<long>();
                var userId = Fixture.Create<long>();

                Mock.Mock<IUserService>()
                    .Setup(service => service.GetCurrentUserId())
                    .Returns(userId);

                Mock.Mock<IDisabledMovieRepository>()
                    .Setup(repository => repository.IsMovieDisabledAsync(movieId, userId))
                    .ReturnsAsync(false);

                // Act

                var result = await ServiceUnderTest.IsMovieDisabledForGivenUserAsync(movieId);

                // Assert
                result
                    .Should()
                    .BeFalse();
            }

            [Fact]
            public async Task Should_return_false_when_user_is_not_logged_in()
            {
                // Arrange
                var movieId = Fixture.Create<long>();
                var userId = Fixture.Create<long>();

                Mock.Mock<IUserService>()
                    .Setup(service => service.GetCurrentUserId())
                    .Returns(0);

                Mock.Mock<IDisabledMovieRepository>()
                    .Setup(repository => repository.IsMovieDisabledAsync(movieId, userId))
                    .Verifiable();

                // Act

                var result = await ServiceUnderTest.IsMovieDisabledForGivenUserAsync(movieId);

                // Assert
                result
                    .Should()
                    .BeFalse();

                Mock.Mock<IDisabledMovieRepository>()
                    .Verify(r => r.IsMovieDisabledAsync(movieId, It.IsAny<long>()), Times.Never);
            }
        }
    }
}