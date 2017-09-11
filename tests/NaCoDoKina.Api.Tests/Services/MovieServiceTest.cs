using FluentAssertions;
using Moq;
using NaCoDoKina.Api.Exceptions;
using NaCoDoKina.Api.Infrastructure.Recommendation.Services;
using NaCoDoKina.Api.Models;
using NaCoDoKina.Api.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Services
{
    public class MovieServiceTest : ServiceWithRepositoryTestBase<IMovieService, IMovieRepository>
    {
        protected Mock<ICinemaService> CinemaServiceMock { get; }

        protected Mock<IRecommendationService> RecommendationServiceMock { get; }

        protected Mock<IUserService> UserServiceMock { get; }

        public MovieServiceTest()
        {
            UserServiceMock = new Mock<IUserService>();
            RecommendationServiceMock = new Mock<IRecommendationService>();
            CinemaServiceMock = new Mock<ICinemaService>();
            RepositoryMock = new Mock<IMovieRepository>();
            ServiceUnderTest = new MovieService(RepositoryMockObject, CinemaServiceMock.Object,
                RecommendationServiceMock.Object, UserServiceMock.Object, MapperMock.Object);

            PreConfigureMocks();
        }

        protected long DefaultUserId = 99;

        protected void PreConfigureMocks()
        {
            UserServiceMock.Setup(service => service.GetCurrentUserId())
                .Returns(Task.FromResult(DefaultUserId));

            MapperMock
                .Setup(mapper => mapper.Map<Movie>(It.IsAny<Entities.Movie>()))
                .Returns(new Func<Entities.Movie, Movie>(movie => new Movie
                {
                    Id = movie.Id,
                    Name = movie.Title,
                }));

            MapperMock
                .Setup(mapper => mapper.Map<Entities.Movie>(It.IsAny<Movie>()))
                .Returns(new Func<Movie, Entities.Movie>(movie => new Entities.Movie
                {
                    Id = movie.Id,
                    Title = movie.Name,
                }));

            MapperMock
                .Setup(mapper => mapper.Map<MovieDetails>(It.IsAny<Entities.MovieDetails>()))
                .Returns(new Func<Entities.MovieDetails, MovieDetails>(movieDetails => new MovieDetails
                {
                    MovieId = movieDetails.Id,
                    Description = movieDetails.Description,
                }));

            MapperMock
                .Setup(mapper => mapper.Map<Entities.MovieDetails>(It.IsAny<MovieDetails>()))
                .Returns(new Func<MovieDetails, Entities.MovieDetails>(movieDetails => new Entities.MovieDetails
                {
                    Id = movieDetails.MovieId,
                    Description = movieDetails.Description,
                }));
        }

        public class GetAllMoviesAsync : MovieServiceTest
        {
        }

        public class GetMoviesPlayedInCinemas : MovieServiceTest
        {
        }

        public class GetMovieAsync : MovieServiceTest
        {
            [Fact]
            public async Task Should_get_movie_with_the_same_id()
            {
                //Arrange
                var movieId = 404L;

                RepositoryMock
                    .Setup(repository => repository.GetMovieAsync(movieId))
                    .Returns(new Func<long, Task<Entities.Movie>>(id => Task.FromResult(new Entities.Movie()
                    {
                        Id = id
                    })));

                //Act
                var resultMovie = await ServiceUnderTest.GetMovieAsync(movieId);

                resultMovie.Id.Should().Be(movieId);
            }

            [Fact]
            public void Should_throw_movie_not_found_exception_when_not_found()
            {
                //Arrange
                var movieId = 404L;

                RepositoryMock
                    .Setup(repository => repository.GetMovieAsync(movieId))
                    .Returns(() => Task.FromResult((Entities.Movie)null));

                //Act
                Func<Task<Movie>> action = () => ServiceUnderTest.GetMovieAsync(movieId);

                //Assert
                action.ShouldThrow<MovieNotFoundException>();
            }
        }

        public class DeleteMovieAsync : MovieServiceTest
        {
            [Fact]
            public async Task Should_delete_movie_for_user_when_id_exist()
            {
                //Arrange
                var movieId = 404L;

                RepositoryMock
                    .Setup(repository => repository.DeleteMovieAsync(movieId, DefaultUserId))
                    .Returns(() => Task.FromResult(true));

                RepositoryMock
                    .Setup(repository => repository.GetMovieAsync(movieId))
                    .Returns(() => Task.FromResult((Entities.Movie)null));

                //Act
                await ServiceUnderTest.DeleteMovieAsync(movieId);
                Func<Task<Movie>> action = () => ServiceUnderTest.GetMovieAsync(movieId);

                //Assert
                action.ShouldThrow<MovieNotFoundException>();
            }

            [Fact]
            public void Should_throw_movie_not_found_exception_when_not_deleted()
            {
                //Arrange
                var movieId = 404L;

                RepositoryMock
                    .Setup(repository => repository.DeleteMovieAsync(movieId, DefaultUserId))
                    .Returns(() => Task.FromResult(false));

                //Act
                Func<Task> action = () => ServiceUnderTest.DeleteMovieAsync(movieId);

                //Assert
                action.ShouldThrow<MovieNotFoundException>();
            }
        }

        public class GetMovieDetailsAsync : MovieServiceTest
        {
            [Fact]
            public async Task Should_get_movie_details_with_the_same_id()
            {
                //Arrange
                var movieId = 404L;

                RepositoryMock
                    .Setup(repository => repository.GetMovieDetailsAsync(movieId))
                    .Returns(new Func<long, Task<Entities.MovieDetails>>(id => Task.FromResult(new Entities.MovieDetails()
                    {
                        Id = id
                    })));

                //Act
                var movieDetails = await ServiceUnderTest.GetMovieDetailsAsync(movieId);

                movieDetails.MovieId.Should().Be(movieId);
            }

            [Fact]
            public void Should_throw_movie_details_not_found_exception_when_not_found()
            {
                //Arrange
                var movieId = 404L;

                RepositoryMock
                    .Setup(repository => repository.GetMovieDetailsAsync(movieId))
                    .Returns(() => Task.FromResult((Entities.MovieDetails)null));

                //Act
                Func<Task<MovieDetails>> action = () => ServiceUnderTest.GetMovieDetailsAsync(movieId);

                //Assert
                action.ShouldThrow<MovieDetailsNotFoundException>();
            }
        }

        public class AddMovieAsync : MovieServiceTest
        {
            [Fact]
            public async Task Should_add_movie_and_return_id()
            {
                //Arrange
                var movieId = 404L;
                var movie = new Movie { Id = movieId, Name = "A", EstimatedRating = 1 };

                RepositoryMock
                    .Setup(repository => repository.AddMovieAsync(It.IsAny<Entities.Movie>()))
                    .Returns(new Func<Entities.Movie, Task<long>>(m => Task.FromResult(m.Id)));

                //Act
                var newId = await ServiceUnderTest.AddMovieAsync(movie);

                movieId.Should().Be(newId);
            }

            [Fact]
            public void Should_throw_movie_not_added_exception_when_could_not_be_added()
            {
                //Arrange
                var movieId = 404L;
                var movie = new Movie { Id = movieId, Name = "A", EstimatedRating = 1 };

                RepositoryMock
                    .Setup(repository => repository.AddMovieAsync(It.IsAny<Entities.Movie>()))
                    .Returns(() => Task.FromResult(0L));

                //Act
                Func<Task<long>> action = () => ServiceUnderTest.AddMovieAsync(movie);

                //Assert
                action.ShouldThrow<MovieNotAddedException>();
            }
        }

        public class AddMovieDetailsAsync : MovieServiceTest
        {
            [Fact]
            public async Task Should_add_movie_details_and_return_id()
            {
                //Arrange
                var movieDetailsId = 404L;
                var movieDetails = new MovieDetails { MovieId = movieDetailsId, OriginalTitle = "A" };

                RepositoryMock
                    .Setup(repository => repository.AddMovieDetailsAsync(It.IsAny<Entities.MovieDetails>()))
                    .Returns(new Func<Entities.MovieDetails, Task<long>>(m => Task.FromResult(m.Id)));

                //Act
                var newId = await ServiceUnderTest.AddMovieDetailsAsync(movieDetails);

                movieDetailsId.Should().Be(newId);
            }

            [Fact]
            public void Should_throw_movie_details_not_added_exception_when_could_not_be_added()
            {
                //Arrange
                var movieDetailsId = 404L;
                var movieDetails = new MovieDetails { MovieId = movieDetailsId, OriginalTitle = "A" };

                RepositoryMock
                    .Setup(repository => repository.AddMovieDetailsAsync(It.IsAny<Entities.MovieDetails>()))
                    .Returns(() => Task.FromResult(0L));

                //Act
                Func<Task<long>> action = () => ServiceUnderTest.AddMovieDetailsAsync(movieDetails);

                //Assert
                action.ShouldThrow<MovieDetailsNotAddedException>();
            }
        }
    }
}