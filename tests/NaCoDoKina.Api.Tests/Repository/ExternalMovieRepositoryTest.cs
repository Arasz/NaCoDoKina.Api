using ApplicationCore.Entities.Movies;
using FluentAssertions;
using Infrastructure.Repositories;
using Ploeh.AutoFixture;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Repository
{
    public class ExternalMovieRepositoryTest : ApplicationRepositoryTestBase<ExternalMovieRepository>
    {
        public class GetExternalMoviesByExternalIds : ExternalMovieRepositoryTest
        {
            [Fact]
            public async Task Should_return_all_external_movies_with_given_id()
            {
                // Arrange
                var movie = Fixture.Build<Movie>()
                    .Without(m => m.Id)
                    .Create();
                var commonId = Fixture.Create<string>();
                var externalMoviesWithCommon = Fixture.Build<ExternalMovie>()
                    .Without(em => em.Id)
                    .With(em => em.ExternalId, commonId)
                    .CreateMany(2)
                    .ToArray();

                var externalMovies = Fixture.Build<ExternalMovie>()
                    .CreateMany(4)
                    .ToArray();

                var allExternalMovies = externalMoviesWithCommon
                    .Concat(externalMovies)
                    .ToList();

                movie.ExternalMovies = allExternalMovies;

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Movies.Add(movie);

                    await scope.DbContext.SaveChangesAsync();
                }

                using (CreateContextScope())
                {
                    // Act
                    var receivedExternalMovies = await RepositoryUnderTest.GetExternalMoviesByExternalIdAsync(commonId);

                    // Assert
                    receivedExternalMovies
                        .Should()
                        .HaveSameCount(externalMoviesWithCommon)
                        .And
                        .Contain(externalMoviesWithCommon);
                }
            }

            [Fact]
            public async Task Should_return_empty_when_no_external_movies_are_found()
            {
                // Arrange
                var movie = Fixture.Build<Movie>()
                    .Without(m => m.Id)
                    .Create();
                var commonId = Fixture.Create<string>();

                var externalMovies = Fixture.Build<ExternalMovie>()
                    .CreateMany(4)
                    .ToList();

                movie.ExternalMovies = externalMovies;

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Movies.Add(movie);

                    await scope.DbContext.SaveChangesAsync();
                }

                using (CreateContextScope())
                {
                    // Act
                    var receivedExternalMovies = await RepositoryUnderTest.GetExternalMoviesByExternalIdAsync(commonId);

                    // Assert
                    receivedExternalMovies
                        .Should()
                        .BeEmpty();
                }
            }
        }
    }
}