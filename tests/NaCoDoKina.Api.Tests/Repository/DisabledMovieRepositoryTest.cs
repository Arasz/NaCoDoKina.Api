using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NaCoDoKina.Api.Entities.Movies;
using NaCoDoKina.Api.Repositories;
using Ploeh.AutoFixture;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Repository
{
    public class DisabledMovieRepositoryTest : ApplicationRepositoryTestBase<DisabledMovieRepository>
    {
        public class CreateDisabledMovieAsync : DisabledMovieRepositoryTest
        {
            [Fact]
            public async Task Should_create_new_disabled_movie()
            {
                // Arrange
                var disabledMovie = Fixture.Create<DisabledMovie>();

                // Act

                using (CreateContextScope())
                {
                    var createdDisabledMovie = await RepositoryUnderTest.CreateDisabledMovieAsync(disabledMovie.MovieId, disabledMovie.UserId);
                    // Assert
                    createdDisabledMovie
                        .Should()
                        .NotBeNull();

                    createdDisabledMovie.MovieId.Should().Be(disabledMovie.MovieId);
                    createdDisabledMovie.UserId.Should().Be(disabledMovie.UserId);
                }
            }
        }

        public class DeleteDisabledMovieAsync : DisabledMovieRepositoryTest
        {
            [Fact]
            public async Task Should_delete_disabled_movie_and_return_true()
            {
                // Arrange
                var userId = Fixture.Create<long>();

                var disabledMovies = Fixture.Build<DisabledMovie>()
                    .With(movie => movie.UserId, userId)
                    .CreateMany(5)
                    .ToArray();

                var movieId = disabledMovies[0].MovieId;

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.DisabledMovies.AddRange(disabledMovies);
                    await scope.DbContext.SaveChangesAsync();
                }

                // Act

                using (CreateContextScope())
                {
                    var isDeleted = await RepositoryUnderTest.DeleteDisabledMovieAsync(movieId, userId);

                    // Assert
                    isDeleted
                        .Should()
                        .BeTrue();
                }

                using (var scope = CreateContextScope())
                {
                    var exist = await scope.DbContext.DisabledMovies
                        .Where(movie => movie.UserId == userId)
                        .Where(movie => movie.MovieId == movieId)
                        .AnyAsync();

                    exist
                        .Should()
                        .BeFalse();
                }
            }

            [Fact]
            public async Task Should_return_false_when_disabled_movie_not_found()
            {
                // Arrange
                var userId = Fixture.Create<long>();

                var disabledMovies = Fixture.Build<DisabledMovie>()
                    .With(movie => movie.UserId, userId)
                    .CreateMany(5)
                    .ToArray();

                var movieId = Fixture.Create<long>();

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.DisabledMovies.AddRange(disabledMovies);
                    await scope.DbContext.SaveChangesAsync();
                }

                // Act

                using (CreateContextScope())
                {
                    var isDeleted = await RepositoryUnderTest.DeleteDisabledMovieAsync(movieId, userId);

                    // Assert
                    isDeleted
                        .Should()
                        .BeFalse();
                }
            }
        }

        public class GetDisabledMoviesIdsForUserAsync : DisabledMovieRepositoryTest
        {
            [Fact]
            public async Task Should_return_all_disabled_movies_for_user()
            {
                // Arrange
                var userId = Fixture.Create<long>();

                var disabledMovies = Fixture.Build<DisabledMovie>()
                    .With(movie => movie.UserId, userId)
                    .CreateMany(5)
                    .ToArray();

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.DisabledMovies.AddRange(disabledMovies);
                    await scope.DbContext.SaveChangesAsync();
                }

                // Act

                using (CreateContextScope())
                {
                    var resultDisabledMovies = await RepositoryUnderTest.GetDisabledMoviesIdsForUserAsync(userId);

                    // Assert
                    resultDisabledMovies
                        .Should()
                        .HaveSameCount(disabledMovies);
                }
            }
        }

        public class IsMovieDisabledAsync : DisabledMovieRepositoryTest
        {
            [Fact]
            public async Task Should_return_true_if_movie_is_disabled()
            {
                // Arrange
                var userId = Fixture.Create<long>();

                var disabledMovies = Fixture.Build<DisabledMovie>()
                    .With(movie => movie.UserId, userId)
                    .CreateMany(5)
                    .ToArray();

                var movieId = disabledMovies[0].MovieId;

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.DisabledMovies.AddRange(disabledMovies);
                    await scope.DbContext.SaveChangesAsync();
                }

                // Act

                using (CreateContextScope())
                {
                    var result = await RepositoryUnderTest.IsMovieDisabledAsync(movieId, userId);

                    // Assert
                    result
                        .Should()
                        .BeTrue();
                }
            }

            [Fact]
            public async Task Should_return_false_if_movie_is_disabled()
            {
                // Arrange
                var userId = Fixture.Create<long>();

                var disabledMovies = Fixture.Build<DisabledMovie>()
                    .With(movie => movie.UserId, userId)
                    .CreateMany(5)
                    .ToArray();

                var movieId = Fixture.Create<long>();

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.DisabledMovies.AddRange(disabledMovies);
                    await scope.DbContext.SaveChangesAsync();
                }

                // Act

                using (CreateContextScope())
                {
                    var result = await RepositoryUnderTest.IsMovieDisabledAsync(movieId, userId);

                    // Assert
                    result
                        .Should()
                        .BeFalse();
                }
            }
        }
    }
}