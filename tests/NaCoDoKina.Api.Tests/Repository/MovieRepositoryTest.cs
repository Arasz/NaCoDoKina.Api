using FluentAssertions;
using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Repositories;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Repository
{
    public class MovieRepositoryTest : RepositoryTestBase<IMovieRepository>
    {
        public class SoftDeleteMovieAsync : MovieRepositoryTest
        {
            [Fact]
            public async Task Should_delete_movie_and_return_true_when_movie_exist()
            {
                using (var databaseScope = new InMemoryDatabaseScope())
                {
                    EnsureCreated(databaseScope);

                    //Arrange
                    var movieId = 1;
                    var userId = 55;

                    var movie = new Movie
                    {
                        Name = nameof(Movie),
                        Details = new MovieDetails(),
                        PosterUrl = nameof(Movie.PosterUrl),
                    };

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Movies.Add(movie);
                        await contextScope.ApplicationContext.SaveChangesAsync();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        RepositoryUnderTest = new MovieRepository(contextScope.ApplicationContext, LoggerMock.Object);

                        //Act
                        var deleted = await RepositoryUnderTest.SoftDeleteMovieAsync(movieId, userId);

                        //Assert
                        deleted.Should().BeTrue();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Movies
                            .Any(m => m.Id == movieId)
                            .Should().BeTrue();
                        contextScope.ApplicationContext.DeletedMovieMarks
                            .Should().HaveCount(1);
                        contextScope.ApplicationContext.DeletedMovieMarks
                            .Should().ContainSingle(mark => mark.MovieId == movieId && mark.UserId == userId);
                    }
                }
            }

            [Fact]
            public async Task Should_return_false_when_movie_do_not_exist()
            {
                using (var databaseScope = new InMemoryDatabaseScope())
                {
                    EnsureCreated(databaseScope);

                    //Arrange
                    var movieId = 1;
                    var nonExistingId = 53;
                    var userId = 55;

                    var movie = new Movie
                    {
                        Name = nameof(Movie),
                        Details = new MovieDetails(),
                        PosterUrl = nameof(Movie.PosterUrl),
                    };

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Movies.Add(movie);
                        await contextScope.ApplicationContext.SaveChangesAsync();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        RepositoryUnderTest = new MovieRepository(contextScope.ApplicationContext, LoggerMock.Object);

                        //Act
                        var deleted = await RepositoryUnderTest.SoftDeleteMovieAsync(nonExistingId, userId);

                        //Assert
                        deleted.Should().BeFalse();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Movies
                            .Any(m => m.Id == movieId)
                            .Should().BeTrue();
                        contextScope.ApplicationContext.DeletedMovieMarks
                            .Should().BeEmpty();
                    }
                }
            }

            [Fact]
            public async Task Should_return_true_if_already_deleted()
            {
                using (var databaseScope = new InMemoryDatabaseScope())
                {
                    EnsureCreated(databaseScope);

                    //Arrange
                    var movieId = 1;
                    var userId = 55;

                    var movie = new Movie
                    {
                        Name = nameof(Movie),
                        Details = new MovieDetails(),
                        PosterUrl = nameof(Movie.PosterUrl),
                    };

                    var deletedMovieMark = new DeletedMovieMark(movieId, userId);

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Movies.Add(movie);
                        contextScope.ApplicationContext.DeletedMovieMarks.Add(deletedMovieMark);
                        await contextScope.ApplicationContext.SaveChangesAsync();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        RepositoryUnderTest = new MovieRepository(contextScope.ApplicationContext, LoggerMock.Object);

                        //Act
                        var deleted = await RepositoryUnderTest.SoftDeleteMovieAsync(movieId, userId);

                        //Assert
                        deleted.Should().BeTrue();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Movies
                            .Any(m => m.Id == movieId)
                            .Should().BeTrue();
                        contextScope.ApplicationContext.DeletedMovieMarks
                            .Should().HaveCount(1);
                        contextScope.ApplicationContext.DeletedMovieMarks
                            .Should()
                            .ContainSingle(mark => mark.MovieId == deletedMovieMark.MovieId && mark.UserId == userId);
                    }
                }
            }
        }

        public class GetMoviesPlayedInCinemaAsync : MovieRepositoryTest
        {
            [Fact]
            public void Should_return_all_not_deleted_movies_played_in_cinema()
            {
            }
        }

        public class DeleteMovieAsync : MovieRepositoryTest
        {
            [Fact]
            public async Task Should_return_true_and_remove_movie_and_details_when_deleted()
            {
                using (var databaseScope = new InMemoryDatabaseScope())
                {
                    EnsureCreated(databaseScope);

                    //Arrange
                    var movieId = 1;

                    var movie = new Movie
                    {
                        Name = nameof(Movie),
                        Details = new MovieDetails(),
                        PosterUrl = nameof(Movie.PosterUrl),
                    };

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Movies.Add(movie);

                        await contextScope.ApplicationContext.SaveChangesAsync();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        RepositoryUnderTest = new MovieRepository(contextScope.ApplicationContext, LoggerMock.Object);

                        //Act
                        var deleted = await RepositoryUnderTest.DeleteMovieAsync(movieId);

                        //Assert
                        deleted.Should().BeTrue();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Movies
                            .Should().BeEmpty();
                        contextScope.ApplicationContext.MovieDetails
                            .Should().BeEmpty();
                    }
                }
            }

            [Fact]
            public async Task Should_return_false_when_movie_can_not_be_found()
            {
                using (var databaseScope = new InMemoryDatabaseScope())
                {
                    EnsureCreated(databaseScope);

                    //Arrange
                    var movieId = 2;

                    var movie = new Movie
                    {
                        Name = nameof(Movie),
                        Details = new MovieDetails(),
                        PosterUrl = nameof(Movie.PosterUrl),
                    };

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Movies.Add(movie);

                        await contextScope.ApplicationContext.SaveChangesAsync();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        RepositoryUnderTest = new MovieRepository(contextScope.ApplicationContext, LoggerMock.Object);

                        //Act
                        var deleted = await RepositoryUnderTest.DeleteMovieAsync(movieId);

                        //Assert
                        deleted.Should().BeFalse();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Movies
                            .Should().HaveCount(1);
                    }
                }
            }
        }
    }
}