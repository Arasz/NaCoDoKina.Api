using FluentAssertions;
using Moq;
using NaCoDoKina.Api.Exceptions;
using NaCoDoKina.Api.Infrastructure.Recommendation.DataContract;
using NaCoDoKina.Api.Infrastructure.Recommendation.Services;
using NaCoDoKina.Api.Models;
using NaCoDoKina.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Cinema = NaCoDoKina.Api.Models.Cinema;
using Movie = NaCoDoKina.Api.Models.Movie;
using MovieDetails = NaCoDoKina.Api.Models.MovieDetails;
using SearchArea = NaCoDoKina.Api.Models.SearchArea;

namespace NaCoDoKina.Api.Services
{
    public class MovieServiceTest : ServiceWithRepositoryTestBase<IMovieService, IMovieRepository>
    {
        protected class MovieRepositoryFake : IMovieRepository
        {
            private List<Entities.MovieShowtime> _movieShowtimes = new List<Entities.MovieShowtime>();

            public void SetTestData(IEnumerable<Entities.MovieShowtime> data)
            {
                _movieShowtimes = data as List<Entities.MovieShowtime> ?? data.ToList();
            }

            public Task<bool> DeleteMovieAsync(long movieId, long userId)
            {
                throw new NotImplementedException();
            }

            public Task<Entities.Movie> GetMovieAsync(long id)
            {
                throw new NotImplementedException();
            }

            public Task<IEnumerable<long>> GetMoviesPlayedInCinemaAsync(long cinemaId, DateTime laterThan)
            {
                var moviesIds = _movieShowtimes
                    .Where(showtime => showtime.Cinema.Id == cinemaId)
                    .Where(showtime => showtime.DateTime > laterThan)
                    .Select(showtime => showtime.Movie.Id);

                return Task.FromResult(moviesIds);
            }

            public Task<Entities.MovieDetails> GetMovieDetailsAsync(long id)
            {
                return Task.FromResult(new Entities.MovieDetails { Id = id });
            }

            public Task<long> AddMovieAsync(Entities.Movie newMovie)
            {
                return Task.FromResult(newMovie.Id);
            }

            public Task<long> AddMovieDetailsAsync(Entities.MovieDetails movieDetails)
            {
                return Task.FromResult(movieDetails.Id);
            }
        }

        protected Mock<ICinemaService> CinemaServiceMock { get; set; }

        protected Mock<IRecommendationService> RecommendationServiceMock { get; set; }

        protected Mock<IUserService> UserServiceMock { get; set; }

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
            private readonly MovieRepositoryFake _movieRepositoryFake;

            public GetAllMoviesAsync()
            {
                _movieRepositoryFake = new MovieRepositoryFake();

                ServiceUnderTest = new MovieService(_movieRepositoryFake, CinemaServiceMock.Object,
                    RecommendationServiceMock.Object, UserServiceMock.Object, MapperMock.Object);

                PreConfigureMocks();
            }

            [Fact]
            public async Task Should_get_all_movies_played_in_area_ordered_by_ratings()
            {
                //Arrange
                var moviesIds = new long[] { 1, 2, 3, 4, 404 };
                var cinemasIdsWithDuration = new(long, long)[] { (1, 10), (2, 15), (3, 18), (4, 9), (69, 44) };
                var now = DateTime.Now;

                var searchArea = new SearchArea(new Location(1, 1), 30);

                var movies = moviesIds
                    .Select(id => new Entities.Movie() { Id = id });

                var cinemas = cinemasIdsWithDuration
                    .Select(tuple => new Cinema
                    {
                        Id = tuple.Item1,
                        CinemaTravelInformation = new TravelInformation(null, 0, TimeSpan.FromMinutes(tuple.Item2))
                    }).ToArray();

                var showTimes = movies.Zip(cinemas, (movie, cinema) => new Entities.MovieShowtime
                {
                    Movie = movie,
                    Cinema = new Entities.Cinema
                    {
                        Id = cinema.Id,
                    },
                    DateTime = now.AddMinutes(20),
                });

                _movieRepositoryFake.SetTestData(showTimes);

                RecommendationServiceMock
                    .Setup(service => service.GetMovieRating(It.IsAny<RecommendationApiRequest>()))
                    .Returns(new Func<RecommendationApiRequest, Task<RecommendationApiResponse>>((request) => Task.FromResult(new RecommendationApiResponse
                    {
                        MovieId = request.MovieId,
                        Rating = request.MovieId
                    })));

                CinemaServiceMock
                    .Setup(service => service.GetNearestCinemasAsync(searchArea))
                    .Returns(() => Task.FromResult(cinemas.AsEnumerable()));

                //Act
                var allMoviesId = await ServiceUnderTest.GetAllMoviesAsync(searchArea);

                //Assert
                allMoviesId.Should().Contain(moviesIds.TakeWhile(id => id < 100));
                allMoviesId.Should().BeInDescendingOrder();
            }

            [Fact]
            public void Should_throw_movies_not_found_exception_when_no_movies_are_played()
            {
                //Arrange
                var moviesIds = new long[] { 1, 2, 3, 4, 404 };
                var cinemasIdsWithDuration = new(long, long)[] { (1, 10), (2, 15), (3, 18), (4, 9), (69, 44) };
                var moviesShowtime = DateTime.Now.AddMinutes(2);

                var searchArea = new SearchArea(new Location(1, 1), 30);

                var movies = moviesIds
                    .Select(id => new Entities.Movie() { Id = id });

                var cinemas = cinemasIdsWithDuration
                    .Select(tuple => new Cinema
                    {
                        Id = tuple.Item1,
                        CinemaTravelInformation = new TravelInformation(null, 0, TimeSpan.FromMinutes(tuple.Item2))
                    }).ToArray();

                var showTimes = movies.Zip(cinemas, (movie, cinema) => new Entities.MovieShowtime
                {
                    Movie = movie,
                    Cinema = new Entities.Cinema
                    {
                        Id = cinema.Id,
                    },
                    DateTime = moviesShowtime,
                });

                _movieRepositoryFake.SetTestData(showTimes);

                RecommendationServiceMock
                    .Setup(service => service.GetMovieRating(It.IsAny<RecommendationApiRequest>()))
                    .Returns(new Func<RecommendationApiRequest, Task<RecommendationApiResponse>>((request) => Task.FromResult(new RecommendationApiResponse
                    {
                        MovieId = request.MovieId,
                        Rating = request.MovieId
                    })));

                CinemaServiceMock
                    .Setup(service => service.GetNearestCinemasAsync(searchArea))
                    .Returns(() => Task.FromResult(cinemas.AsEnumerable()));

                //Act
                Func<Task<IEnumerable<long>>> action = () => ServiceUnderTest.GetAllMoviesAsync(searchArea);

                //Assert
                action.ShouldThrow<MoviesNotFoundException>();
            }
        }

        public class GetMoviesPlayedInCinemas : MovieServiceTest
        {
            private readonly MovieRepositoryFake _movieRepositoryFake;

            public GetMoviesPlayedInCinemas()
            {
                _movieRepositoryFake = new MovieRepositoryFake();

                ServiceUnderTest = new MovieService(_movieRepositoryFake, CinemaServiceMock.Object,
                    RecommendationServiceMock.Object, UserServiceMock.Object, MapperMock.Object);

                PreConfigureMocks();
            }

            [Fact]
            public async Task Should_get_all_movies_played_in_cinema_ordered_by_ratings()
            {
                //Arrange
                var moviesIds = new long[] { 1, 2, 3, 4, 404 };
                var cinemasIdsWithDuration = new(long, long)[] { (1, 10), (2, 15), (3, 18), (4, 9), (69, 44) };
                var now = DateTime.Now;

                var movies = moviesIds
                    .Select(id => new Entities.Movie() { Id = id });

                var cinemas = cinemasIdsWithDuration
                    .Select(tuple => new Cinema
                    {
                        Id = tuple.Item1,
                        CinemaTravelInformation = new TravelInformation(null, 0, TimeSpan.FromMinutes(tuple.Item2))
                    }).ToArray();

                var showTimes = movies.Zip(cinemas, (movie, cinema) => new Entities.MovieShowtime
                {
                    Movie = movie,
                    Cinema = new Entities.Cinema
                    {
                        Id = cinema.Id,
                    },
                    DateTime = now.AddMinutes(20),
                });

                _movieRepositoryFake.SetTestData(showTimes);

                RecommendationServiceMock
                    .Setup(service => service.GetMovieRating(It.IsAny<RecommendationApiRequest>()))
                    .Returns(new Func<RecommendationApiRequest, Task<RecommendationApiResponse>>((request) => Task.FromResult(new RecommendationApiResponse
                    {
                        MovieId = request.MovieId,
                        Rating = request.MovieId
                    })));

                //Act
                var playedMoviesIds = await ServiceUnderTest.GetMoviesPlayedInCinemas(cinemas);

                //Assert
                playedMoviesIds.Should().Contain(moviesIds.TakeWhile(id => id < 100));
                playedMoviesIds.Should().BeInDescendingOrder();
            }
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