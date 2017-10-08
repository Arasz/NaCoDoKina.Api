using FluentAssertions;
using Moq;
using NaCoDoKina.Api.Entities.Cinemas;
using NaCoDoKina.Api.Entities.Movies;
using NaCoDoKina.Api.Repositories;
using NaCoDoKina.Api.Services;
using Ploeh.AutoFixture;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Repository
{
    public class MovieRepositoryTest : ApplicationRepositoryTestBase<IMovieRepository>
    {
        public Mock<IUserService> UserServiceMock { get; }

        public long DefaultUserId { get; }

        public MovieRepositoryTest()
        {
            UserServiceMock = new Mock<IUserService>();

            DefaultUserId = Fixture.Create<long>();

            UserServiceMock
                .Setup(service => service.GetCurrentUserId())
                .Returns(DefaultUserId);
        }

        public class CreateMovieAsync : MovieRepositoryTest
        {
            [Fact]
            public async Task Should_add_movie_and_return_id()
            {
                //Arrange
                var movie = Fixture.Build<Movie>()
                    .Without(m => m.Id)
                    .Create();

                var movieId = 0L;

                using (var contextScope = CreateContextScope())
                {
                    RepositoryUnderTest =
                        new MovieRepository(contextScope.DbContext, LoggerMock.Object);

                    //Act
                    movieId = await RepositoryUnderTest.CreateMovieAsync(movie);

                    //Assert
                    movieId.Should().BePositive();
                }

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.Movies
                        .Any(m => m.Id == movieId)
                        .Should().BeTrue();
                }
            }
        }

        public class CreateMoviesAsync : MovieRepositoryTest
        {
            [Fact]
            public async Task Should_add_movies()
            {
                //Arrange
                var moviesCount = 5;
                var movies = Fixture.Build<Movie>()
                    .Without(m => m.Id)
                    .CreateMany(moviesCount);

                using (var contextScope = CreateContextScope())
                {
                    RepositoryUnderTest =
                        new MovieRepository(contextScope.DbContext, LoggerMock.Object);

                    //Act
                    await RepositoryUnderTest.CreateMoviesAsync(movies);
                }

                //Assert
                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.Movies
                        .Count().Should().Be(moviesCount);
                }
            }
        }

        public class AddMovieDetailsAsync : MovieRepositoryTest
        {
            [Fact]
            public async Task Should_add_movie_details_and_return_id()
            {
                //Arrange
                var movie = Fixture.Build<Movie>()
                    .Without(m => m.Id)
                    .Create();

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.Movies.Add(movie);
                    await contextScope.DbContext.SaveChangesAsync();
                }

                var movieDetails = Fixture.Build<MovieDetails>()
                    .With(details => details.Id, movie.Id)
                    .Create();

                using (var contextScope = CreateContextScope())
                {
                    RepositoryUnderTest =
                        new MovieRepository(contextScope.DbContext, LoggerMock.Object);

                    //Act
                    var addedDetailsId = await RepositoryUnderTest.CreateMovieDetailsAsync(movieDetails);

                    //Assert
                    addedDetailsId.Should().Be(movie.Id);
                }

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.Movies
                        .Any(m => m.Id == movie.Id)
                        .Should().BeTrue();

                    contextScope.DbContext.MovieDetails
                        .Any(details => details.Id == movie.Id)
                        .Should().BeTrue();

                    contextScope.DbContext.MovieDetails
                        .Single()
                        .Description.Should().Be(movieDetails.Description);
                }
            }

            [Fact]
            public async Task Should_return_0_id_when_movie_can_not_be_found()
            {
                //Arrange
                var notExistingMovieId = 404L;

                //Arrange
                var movie = Fixture.Build<Movie>()
                    .Without(m => m.Id)
                    .Create();

                var movieDetails = Fixture.Build<MovieDetails>()
                    .With(details => details.Id, notExistingMovieId)
                    .Create();

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.Movies.Add(movie);
                    await contextScope.DbContext.SaveChangesAsync();
                }

                using (var contextScope = CreateContextScope())
                {
                    RepositoryUnderTest =
                        new MovieRepository(contextScope.DbContext, LoggerMock.Object);

                    //Act
                    var addedDetailsId = await RepositoryUnderTest.CreateMovieDetailsAsync(movieDetails);

                    //Assert
                    addedDetailsId.Should().Be(0);
                }

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.Movies
                        .Any(m => m.Id == movie.Id)
                        .Should().BeTrue();

                    contextScope.DbContext.MovieDetails
                        .Should()
                        .OnlyContain(details => details.Description == movie.Details.Description);
                }
            }
        }

        public class SoftDeleteMovieAsync : MovieRepositoryTest
        {
            [Fact]
            public async Task Should_delete_movie_and_return_true_when_movie_exist()
            {
                //Arrange
                var movie = Fixture.Build<Movie>()
                    .Without(m => m.Id)
                    .Create();

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.Movies.Add(movie);
                    await contextScope.DbContext.SaveChangesAsync();
                }

                using (var contextScope = CreateContextScope())
                {
                    RepositoryUnderTest =
                        new MovieRepository(contextScope.DbContext, LoggerMock.Object);

                    //Act
                    var deleted = await RepositoryUnderTest.DisableMovieForUserAsync(movie.Id, TODO);

                    //Assert
                    deleted.Should().BeTrue();
                }

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.Movies
                        .Any(m => m.Id == movie.Id)
                        .Should().BeTrue();
                    contextScope.DbContext.DisabledMovies
                        .Should().HaveCount(1);
                    contextScope.DbContext.DisabledMovies
                        .Should().ContainSingle(mark => mark.MovieId == movie.Id && mark.UserId == DefaultUserId);
                }
            }

            [Fact]
            public async Task Should_return_false_when_movie_do_not_exist()
            {
                //Arrange
                var nonExistingId = 53;

                //Arrange
                var movie = Fixture.Build<Movie>()
                    .Without(m => m.Id)
                    .Create();

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.Movies.Add(movie);
                    await contextScope.DbContext.SaveChangesAsync();
                }

                using (var contextScope = CreateContextScope())
                {
                    RepositoryUnderTest =
                        new MovieRepository(contextScope.DbContext, LoggerMock.Object);

                    //Act
                    var deleted = await RepositoryUnderTest.DisableMovieForUserAsync(nonExistingId, TODO);

                    //Assert
                    deleted.Should().BeFalse();
                }

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.Movies
                        .Any(m => m.Id == movie.Id)
                        .Should().BeTrue();
                    contextScope.DbContext.DisabledMovies
                        .Should().BeEmpty();
                }
            }

            [Fact]
            public async Task Should_return_true_if_already_deleted()
            {
                //Arrange
                var movie = Fixture.Build<Movie>()
                    .Without(m => m.Id)
                    .Create();

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.Movies.Add(movie);
                    await contextScope.DbContext.SaveChangesAsync();
                }

                var deletedMovieMark = new DisabledMovie(movie.Id, DefaultUserId);

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.DisabledMovies.Add(deletedMovieMark);
                    await contextScope.DbContext.SaveChangesAsync();
                }

                using (var contextScope = CreateContextScope())
                {
                    RepositoryUnderTest =
                        new MovieRepository(contextScope.DbContext, LoggerMock.Object);

                    //Act
                    var deleted = await RepositoryUnderTest.DisableMovieForUserAsync(movie.Id, TODO);

                    //Assert
                    deleted.Should().BeTrue();
                }

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.Movies
                        .Any(m => m.Id == movie.Id)
                        .Should().BeTrue();
                    contextScope.DbContext.DisabledMovies
                        .Should().HaveCount(1);
                    contextScope.DbContext.DisabledMovies
                        .Should()
                        .ContainSingle(mark =>
                            mark.MovieId == deletedMovieMark.MovieId && mark.UserId == DefaultUserId);
                }
            }
        }
    }

    public class GetMovieByExternalIdAsync : MovieRepositoryTest
    {
        [Fact]
        public async Task Should_return_movie_when_exist()
        {
            //Arrange
            var externalMovies = Fixture.Build<ExternalMovie>()
                .Without(em => em.Id)
                .CreateMany(1)
                .ToList();

            var externalMovie = externalMovies.First();

            var movie = Fixture.Build<Movie>()
                .Without(m => m.Id)
                .With(m => m.ExternalMovies, externalMovies)
                .Create();

            using (var contextScope = CreateContextScope())
            {
                contextScope.DbContext.Movies.Add(movie);

                await contextScope.DbContext.SaveChangesAsync();
            }

            using (var contextScope = CreateContextScope())
            {
                RepositoryUnderTest =
                    new MovieRepository(contextScope.DbContext, LoggerMock.Object);

                //Act
                var movieFromDb = await RepositoryUnderTest.GetMovieByExternalIdAsync(externalMovie.ExternalId);

                //Assert
                movieFromDb.Id.Should().BePositive();
                movieFromDb.Details.Should().BeNull("We only get necessary data");
                movieFromDb.Title.Should().Be(movie.Title);
                movieFromDb.ExternalMovies
                    .Should()
                    .HaveSameCount(externalMovies)
                    .And
                    .Match(movies => movies.All(em => em.ExternalId == externalMovie.ExternalId));
            }
        }

        [Fact]
        public async Task Should_return_null_when_movie_do_not_exist()
        {
            //Arrange
            var nonExistingMovieId = Fixture.Create<string>();

            var movie = Fixture.Build<Movie>()
                .Without(m => m.Id)
                .Create();

            using (var contextScope = CreateContextScope())
            {
                contextScope.DbContext.Movies.Add(movie);

                await contextScope.DbContext.SaveChangesAsync();
            }

            using (var contextScope = CreateContextScope())
            {
                RepositoryUnderTest =
                    new MovieRepository(contextScope.DbContext, LoggerMock.Object);

                //Act
                var movieFromDb = await RepositoryUnderTest.GetMovieByExternalIdAsync(nonExistingMovieId);

                //Assert
                movieFromDb.Should().BeNull();
            }
        }
    }

    public class GetMovieAsync : MovieRepositoryTest
    {
        [Fact]
        public async Task Should_return_movie_when_exist_and_is_not_marked_as_deleted()
        {
            //Arrange
            var movie = Fixture.Build<Movie>()
                .Without(m => m.Id)
                .Create();

            using (var contextScope = CreateContextScope())
            {
                contextScope.DbContext.Movies.Add(movie);

                await contextScope.DbContext.SaveChangesAsync();
            }

            using (var contextScope = CreateContextScope())
            {
                RepositoryUnderTest =
                    new MovieRepository(contextScope.DbContext, LoggerMock.Object);

                //Act
                var movieFromDb = await RepositoryUnderTest.GetMovieAsync(movie.Id);

                //Assert
                movieFromDb.Id.Should().BePositive();
                movieFromDb.Details.Should().BeNull("We only get necessary data");
                movieFromDb.Title.Should().Be(movie.Title);
            }
        }

        [Fact]
        public async Task Should_return_null_when_movie_do_not_exist()
        {
            //Arrange
            var nonExistingMovieId = Fixture.Create<long>();

            var movie = Fixture.Build<Movie>()
                .Without(m => m.Id)
                .Create();

            using (var contextScope = CreateContextScope())
            {
                contextScope.DbContext.Movies.Add(movie);

                await contextScope.DbContext.SaveChangesAsync();
            }

            using (var contextScope = CreateContextScope())
            {
                RepositoryUnderTest =
                    new MovieRepository(contextScope.DbContext, LoggerMock.Object);

                //Act
                var movieFromDb = await RepositoryUnderTest.GetMovieAsync(nonExistingMovieId);

                //Assert
                movieFromDb.Should().BeNull();
            }
        }

        [Fact]
        public async Task Should_return_null_when_movie_is_soft_deleted()
        {
            //Arrange
            var movie = Fixture.Create<Movie>();
            var movieId = movie.Id;

            var deletedMovieMark = new DisabledMovie(movieId, DefaultUserId);

            using (var contextScope = CreateContextScope())
            {
                contextScope.DbContext.Movies.Add(movie);
                contextScope.DbContext.DisabledMovies.Add(deletedMovieMark);
                await contextScope.DbContext.SaveChangesAsync();
            }

            using (var contextScope = CreateContextScope())
            {
                RepositoryUnderTest =
                    new MovieRepository(contextScope.DbContext, LoggerMock.Object);

                //Act
                var movieFromDb = await RepositoryUnderTest.GetMovieAsync(movieId);

                //Assert
                movieFromDb.Should().BeNull();
            }
        }
    }

    public class GetMovieDetailsAsync : MovieRepositoryTest
    {
        [Fact]
        public async Task Should_return_movie_details_when_exist_and_is_not_marked_as_deleted()
        {
            //Arrange
            var movie = Fixture.Build<Movie>()
                .Without(m => m.Id)
                .Create();

            using (var contextScope = CreateContextScope())
            {
                contextScope.DbContext.Movies.Add(movie);

                await contextScope.DbContext.SaveChangesAsync();
            }

            using (var contextScope = CreateContextScope())
            {
                RepositoryUnderTest =
                    new MovieRepository(contextScope.DbContext, LoggerMock.Object);

                //Act
                var movieFromDb = await RepositoryUnderTest.GetMovieDetailsAsync(movie.Id);

                //Assert
                movieFromDb.Id.Should().BePositive();
                movieFromDb.ReleaseDate.Should().Be(movie.Details.ReleaseDate);
            }
        }

        [Fact]
        public async Task Should_return_null_when_movie_details_do_not_exist()
        {
            //Arrange
            var nonExistingMovieId = 52;

            //Arrange
            var movie = Fixture.Create<Movie>();

            using (var contextScope = CreateContextScope())
            {
                contextScope.DbContext.Movies.Add(movie);

                await contextScope.DbContext.SaveChangesAsync();
            }

            using (var contextScope = CreateContextScope())
            {
                RepositoryUnderTest =
                    new MovieRepository(contextScope.DbContext, LoggerMock.Object);

                //Act
                var movieFromDb = await RepositoryUnderTest.GetMovieDetailsAsync(nonExistingMovieId);

                //Assert
                movieFromDb.Should().BeNull();
            }
        }

        [Fact]
        public async Task Should_return_null_when_movie_is_soft_deleted()
        {
            //Arrange
            var movie = Fixture.Create<Movie>();
            var movieId = movie.Id;

            var deletedMovieMark = new DisabledMovie(movieId, DefaultUserId);

            using (var contextScope = CreateContextScope())
            {
                contextScope.DbContext.Movies.Add(movie);
                contextScope.DbContext.DisabledMovies.Add(deletedMovieMark);
                await contextScope.DbContext.SaveChangesAsync();
            }

            using (var contextScope = CreateContextScope())
            {
                RepositoryUnderTest =
                    new MovieRepository(contextScope.DbContext, LoggerMock.Object);

                //Act
                var movieFromDb = await RepositoryUnderTest.GetMovieDetailsAsync(movieId);

                //Assert
                movieFromDb.Should().BeNull();
            }
        }
    }

    public class GetMoviesPlayedInCinemaAsync : MovieRepositoryTest
    {
        [Fact]
        public async Task Should_return_all_not_deleted_movies_ids_played_in_cinema()
        {
            //Arrange
            var movie = Fixture.Build<Movie>()
                .Without(m => m.Id)
                .Create();

            var deletedMovie = Fixture.Create<Movie>();
            var deletedMovieId = deletedMovie.Id;

            var deleteMovieMark = new DisabledMovie(deletedMovieId, DefaultUserId);

            var cinema = Fixture.Create<Cinema>();
            var cinemaId = cinema.Id;

            var movieShowtime = new MovieShowtime
            {
                Cinema = cinema,
                Movie = movie,
                ShowTime = DateTime.Now.AddHours(2),
                Language = "",
                ShowType = ""
            };

            using (var contextScope = CreateContextScope())
            {
                contextScope.DbContext.Cinemas.Add(cinema);
                contextScope.DbContext.MovieShowtimes.Add(movieShowtime);
                contextScope.DbContext.Movies.Add(movie);
                contextScope.DbContext.Movies.Add(deletedMovie);
                contextScope.DbContext.DisabledMovies.Add(deleteMovieMark);

                await contextScope.DbContext.SaveChangesAsync();
            }

            using (var contextScope = CreateContextScope())
            {
                RepositoryUnderTest =
                    new MovieRepository(contextScope.DbContext, LoggerMock.Object);

                //Act
                var playedMoviesIds =
                    await RepositoryUnderTest.GetNotDisabledMoviesForUserAndCinemaAsync(cinemaId, DateTime.Now);

                //Assert
                playedMoviesIds.Should().HaveCount(1);
                playedMoviesIds.Single().Should().Be(movie.Id);
            }

            using (var contextScope = CreateContextScope())
            {
                contextScope.DbContext.Movies
                    .Should().HaveCount(2);
            }
        }
    }

    public class DeleteMovieAsync : MovieRepositoryTest
    {
        [Fact]
        public async Task Should_return_true_and_remove_movie_and_details_when_deleted()
        {
            //Arrange

            var movie = Fixture.Build<Movie>()
                .Without(m => m.Id)
                .Create();

            using (var contextScope = CreateContextScope())
            {
                contextScope.DbContext.Movies.Add(movie);

                await contextScope.DbContext.SaveChangesAsync();
            }

            var movieDeletedMark = new DisabledMovie(movie.Id, DefaultUserId);

            using (var contextScope = CreateContextScope())
            {
                contextScope.DbContext.DisabledMovies.Add(movieDeletedMark);

                await contextScope.DbContext.SaveChangesAsync();
            }

            using (var contextScope = CreateContextScope())
            {
                RepositoryUnderTest =
                    new MovieRepository(contextScope.DbContext, LoggerMock.Object);

                //Act
                var deleted = await RepositoryUnderTest.DeleteMovieAsync(movie.Id);

                //Assert
                deleted.Should().BeTrue();
            }

            using (var contextScope = CreateContextScope())
            {
                contextScope.DbContext.Movies
                    .Should().BeEmpty();
                contextScope.DbContext.MovieDetails
                    .Should().BeEmpty();
                contextScope.DbContext.DisabledMovies
                    .Should().BeEmpty();
            }
        }

        [Fact]
        public async Task Should_return_false_when_movie_can_not_be_found()
        {
            //Arrange
            var movie = Fixture.Create<Movie>();
            var nonExistingMovie = Fixture.Create<long>();

            using (var contextScope = CreateContextScope())
            {
                contextScope.DbContext.Movies.Add(movie);

                await contextScope.DbContext.SaveChangesAsync();
            }

            using (var contextScope = CreateContextScope())
            {
                RepositoryUnderTest =
                    new MovieRepository(contextScope.DbContext, LoggerMock.Object);

                //Act
                var deleted = await RepositoryUnderTest.DeleteMovieAsync(nonExistingMovie);

                //Assert
                deleted.Should().BeFalse();
            }

            using (var contextScope = CreateContextScope())
            {
                contextScope.DbContext.Movies
                    .Should().HaveCount(1);
            }
        }
    }
}