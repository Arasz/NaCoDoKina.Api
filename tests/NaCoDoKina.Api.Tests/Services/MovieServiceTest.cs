using FluentAssertions;
using Moq;
using NaCoDoKina.Api.Exceptions;
using NaCoDoKina.Api.Models;
using NaCoDoKina.Api.Models.Travel;
using NaCoDoKina.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Cinema = NaCoDoKina.Api.Models.Cinemas.Cinema;
using Movie = NaCoDoKina.Api.Models.Movies.Movie;
using MovieDetails = NaCoDoKina.Api.Models.Movies.MovieDetails;
using MovieShowtime = NaCoDoKina.Api.Entities.Movies.MovieShowtime;
using SearchArea = NaCoDoKina.Api.Models.SearchArea;

namespace NaCoDoKina.Api.Services
{
    public class MovieServiceTest : ServiceWithRepositoryTestBase<IMovieService, IMovieRepository>
    {
        protected Mock<ICinemaService> CinemaServiceMock { get; set; }

        protected Mock<IRatingService> RatingServiceMock { get; set; }

        protected Mock<IUserService> UserServiceMock { get; set; }

        public MovieServiceTest()
        {
            UserServiceMock = Mock.Mock<IUserService>();
            RatingServiceMock = Mock.Mock<IRatingService>();
            CinemaServiceMock = Mock.Mock<ICinemaService>();

            ServiceUnderTest = Mock.Create<MovieService>();

            PreConfigureMocks();
        }

        protected long DefaultUserId = 99;

        protected void PreConfigureMocks()
        {
            UserServiceMock.Setup(service => service.GetCurrentUserId())
                .Returns(DefaultUserId);

            MapperMock
                .Setup(mapper => mapper.Map<Movie>(It.IsAny<Entities.Movies.Movie>()))
                .Returns(new Func<Entities.Movies.Movie, Movie>(movie => new Movie
                {
                    Id = movie.Id,
                    Title = movie.Title,
                }));

            Mock.Mock<IDisabledMovieService>()
                .Setup(service => service.FilterDisabledMoviesForCurrentUserAsync(It.IsAny<IEnumerable<long>>()))
                .ReturnsAsync(new Func<IEnumerable<long>, ICollection<long>>(ids => ids.ToArray()));

            MapperMock
                .Setup(mapper => mapper.Map<Entities.Movies.Movie>(It.IsAny<Movie>()))
                .Returns(new Func<Movie, Entities.Movies.Movie>(movie => new Entities.Movies.Movie
                {
                    Id = movie.Id,
                    Title = movie.Title,
                }));

            MapperMock
                .Setup(mapper => mapper.Map<MovieDetails>(It.IsAny<Entities.Movies.MovieDetails>()))
                .Returns(new Func<Entities.Movies.MovieDetails, MovieDetails>(movieDetails => new MovieDetails
                {
                    Id = movieDetails.Id,
                    Description = movieDetails.Description,
                }));

            MapperMock
                .Setup(mapper => mapper.Map<Entities.Movies.MovieDetails>(It.IsAny<MovieDetails>()))
                .Returns(new Func<MovieDetails, Entities.Movies.MovieDetails>(movieDetails => new Entities.Movies.MovieDetails
                {
                    Id = movieDetails.Id,
                    Description = movieDetails.Description,
                }));

            RatingServiceMock
                .Setup(service => service.GetMovieRating(It.IsAny<long>()))
                .Returns(new Func<long, Task<double>>((movieId) => Task.FromResult((double)movieId)));
        }

        public class MovieServiceTestsWithFakeRepository : MovieServiceTest
        {
            protected class MovieRepositoryFake : IMovieRepository
            {
                private List<MovieShowtime> _movieShowtimes = new List<MovieShowtime>();

                public void SetTestData(IEnumerable<MovieShowtime> data)
                {
                    _movieShowtimes = data as List<MovieShowtime> ?? data.ToList();
                }

                public Task<bool> DisableMovieForUserAsync(long movieId, long userId)
                {
                    throw new NotImplementedException();
                }

                public Task<bool> DeleteMovieAsync(long movieId)
                {
                    throw new NotImplementedException();
                }

                public Task<Entities.Movies.Movie> GetMovieAsync(long id)
                {
                    throw new NotImplementedException();
                }

                public async Task<Entities.Movies.Movie> GetMovieByExternalIdAsync(string externalId)
                {
                    throw new NotImplementedException();
                }

                public async Task<IEnumerable<Entities.Movies.Movie>> GetMoviesByExternalIdsAsync(HashSet<string> externalIds)
                {
                    throw new NotImplementedException();
                }

                public Task<IEnumerable<long>> GetNotDisabledMoviesForUserAndCinemaAsync(long cinemaId, DateTime laterThan)
                {
                    var moviesIds = _movieShowtimes
                        .Where(showtime => showtime.Cinema.Id == cinemaId)
                        .Where(showtime => showtime.ShowTime > laterThan)
                        .Select(showtime => showtime.Movie.Id);

                    return Task.FromResult(moviesIds);
                }

                public Task<Entities.Movies.MovieDetails> GetMovieDetailsAsync(long id)
                {
                    return Task.FromResult(new Entities.Movies.MovieDetails { Id = id });
                }

                public async Task CreateMoviesAsync(IEnumerable<Entities.Movies.Movie> movies)
                {
                    throw new NotImplementedException();
                }

                public Task<long> CreateMovieAsync(Entities.Movies.Movie newMovie)
                {
                    return Task.FromResult(newMovie.Id);
                }

                public Task<long> CreateMovieDetailsAsync(Entities.Movies.MovieDetails movieDetails)
                {
                    return Task.FromResult(movieDetails.Id);
                }

                public async Task<IEnumerable<long>> GetMoviesForCinemaAsync(long cinemaId, DateTime laterThan)
                {
                    return _movieShowtimes
                        .Where(showtime => showtime.Cinema.Id == cinemaId)
                        .Select(showtime => showtime.Movie.Id);
                }
            }

            protected readonly MovieRepositoryFake _movieRepositoryFake;
            protected SearchArea SearchArea;
            protected long[] MoviesIds;
            protected (long, long)[] CinemasIdsWithDuration;
            protected DateTime Now;
            protected IEnumerable<Entities.Movies.Movie> Movies;
            protected Cinema[] Cinemas;
            protected IEnumerable<MovieShowtime> ShowTimes;

            public MovieServiceTestsWithFakeRepository()
            {
                _movieRepositoryFake = new MovieRepositoryFake();

                Mock.Provide<IMovieRepository>(_movieRepositoryFake);

                ServiceUnderTest = Mock.Create<MovieService>();

                PreConfigureMocks();
            }

            protected void SetFakeTestData()
            {
                MoviesIds = new long[] { 1, 2, 3, 4, 404 };
                CinemasIdsWithDuration = new(long, long)[] { (1, 10), (2, 15), (3, 18), (4, 9), (69, 44) };
                Now = DateTime.Now;

                SearchArea = new SearchArea(new Location(1, 1), 30);

                Movies = MoviesIds
                    .Select(id => new Entities.Movies.Movie { Id = id });

                Cinemas = CinemasIdsWithDuration
                    .Select(tuple => new Cinema
                    {
                        Id = tuple.Item1,
                        CinemaTravelInformation = new TravelInformation(null, 0, TimeSpan.FromMinutes(tuple.Item2))
                    }).ToArray();

                ShowTimes = Movies.Zip(Cinemas, (movie, cinema) => new MovieShowtime
                {
                    Movie = movie,
                    Cinema = new Entities.Cinemas.Cinema
                    {
                        Id = cinema.Id,
                    },
                    ShowTime = Now.AddMinutes(20),
                });

                _movieRepositoryFake.SetTestData(ShowTimes);
            }
        }

        public class GetAllMoviesAsync : MovieServiceTestsWithFakeRepository
        {
            [Fact]
            public async Task Should_get_all_movies_played_in_area_ordered_by_ratings()
            {
                //Arrange

                SetFakeTestData();

                RatingServiceMock
                    .Setup(service => service.GetMovieRating(It.IsAny<long>()))
                    .Returns(new Func<long, Task<double>>((movieId) => Task.FromResult((double)movieId)));

                CinemaServiceMock
                    .Setup(service => service.GetCinemasInSearchAreaAsync(SearchArea))
                    .ReturnsAsync(Cinemas);

                //Act
                var allMoviesId = await ServiceUnderTest.GetAllMoviesAsync(SearchArea);

                //Assert
                allMoviesId.Should().Contain(MoviesIds.TakeWhile(id => id < 100));
                allMoviesId.Should().BeInDescendingOrder();
            }

            [Fact]
            public void Should_throw_movies_not_found_exception_when_no_movies_are_played()
            {
                //Arrange
                SetFakeTestData();
                _movieRepositoryFake.SetTestData(new List<MovieShowtime>());

                CinemaServiceMock
                    .Setup(service => service.GetCinemasInSearchAreaAsync(SearchArea))
                    .ReturnsAsync(Cinemas);

                //Act
                Func<Task<ICollection<long>>> action = () => ServiceUnderTest.GetAllMoviesAsync(SearchArea);

                //Assert
                action.ShouldThrow<MoviesNotFoundException>();
            }

            [Fact]
            public void Should_throw_cinemas_not_found_exception_when_no_cinemas_are_found()
            {
                //Arrange

                CinemaServiceMock
                    .Setup(service => service.GetCinemasInSearchAreaAsync(SearchArea))
                    .Throws(new CinemasNotFoundException(SearchArea));

                //Act
                Func<Task<ICollection<long>>> action = () => ServiceUnderTest.GetAllMoviesAsync(SearchArea);

                //Assert
                action.ShouldThrow<CinemasNotFoundException>();
            }

            [Fact]
            public void Should_throw_rating_not_found_exception_when_movie_rating_not_found()
            {
                //Arrange
                SetFakeTestData();

                CinemaServiceMock
                    .Setup(service => service.GetCinemasInSearchAreaAsync(SearchArea))
                    .ReturnsAsync(Cinemas);

                RatingServiceMock
                    .Setup(service => service.GetMovieRating(It.IsAny<long>()))
                    .Throws(new RatingNotFoundException(1, 1));

                //Act
                Func<Task<ICollection<long>>> action = () => ServiceUnderTest.GetAllMoviesAsync(SearchArea);

                //Assert
                action.ShouldThrow<RatingNotFoundException>();
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
                    .ReturnsAsync(new Func<long, Entities.Movies.Movie>(id => new Entities.Movies.Movie
                    {
                        Id = id
                    }));

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
                    .Returns(() => Task.FromResult((Entities.Movies.Movie)null));

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

                Mock.Mock<IDisabledMovieService>()
                    .Setup(service => service.DisableMovieForCurrentUserAsync(movieId))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

                RepositoryMock
                    .Setup(repository => repository.GetMovieAsync(movieId))
                    .ReturnsAsync((Entities.Movies.Movie)null);

                //Act
                await ServiceUnderTest.DeleteMovieAsync(movieId);
                Func<Task<Movie>> action = () => ServiceUnderTest.GetMovieAsync(movieId);

                //Assert
                action.ShouldThrow<MovieNotFoundException>();
                Mock.Mock<IDisabledMovieService>()
                    .Verify(s => s.DisableMovieForCurrentUserAsync(movieId));
            }
        }

        public class GetMovieDetailsAsync : MovieServiceTest
        {
            [Fact]
            public async Task Should_get_movie_details_with_the_same_id_and_rating()
            {
                //Arrange
                var movieId = 404L;

                RepositoryMock
                    .Setup(repository => repository.GetMovieDetailsAsync(movieId))
                    .ReturnsAsync(new Func<long, Entities.Movies.MovieDetails>(id => new Entities.Movies.MovieDetails
                    {
                        Id = id,
                    }));

                //Act
                var movieDetails = await ServiceUnderTest.GetMovieDetailsAsync(movieId);

                movieDetails.Id.Should().Be(movieId);
                movieDetails.Rating.Should().Be(movieId);
            }

            [Fact]
            public async Task When_movie_rating_not_found_should_return_movie_details_and_0_rating()
            {
                //Arrange
                var movieId = 404L;

                RatingServiceMock
                    .Setup(service => service.GetMovieRating(movieId))
                    .Throws(new RatingNotFoundException(movieId, 0));

                RepositoryMock
                    .Setup(repository => repository.GetMovieDetailsAsync(movieId))
                    .ReturnsAsync(new Func<long, Entities.Movies.MovieDetails>(id => new Entities.Movies.MovieDetails
                    {
                        Id = id,
                    }));

                //Act
                var movieDetails = await ServiceUnderTest.GetMovieDetailsAsync(movieId);

                movieDetails.Id.Should().Be(movieId);
                movieDetails.Rating.Should().Be(0);
            }

            [Fact]
            public void Should_throw_movie_details_not_found_exception_when_not_found()
            {
                //Arrange
                var movieId = 404L;

                RepositoryMock
                    .Setup(repository => repository.GetMovieDetailsAsync(movieId))
                    .ReturnsAsync(() => (Entities.Movies.MovieDetails)null);

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
                var movie = new Movie { Id = movieId, Title = "A" };
                RepositoryMock
                    .Setup(repository => repository.CreateMovieAsync(It.IsAny<Entities.Movies.Movie>()))
                    .ReturnsAsync(new Func<Entities.Movies.Movie, long>(m => m.Id));

                //Act
                var newId = await ServiceUnderTest.AddMovieAsync(movie);

                movieId.Should().Be(newId);
            }

            [Fact]
            public void Should_throw_movie_not_added_exception_when_could_not_be_added()
            {
                //Arrange
                var movieId = 404L;
                var movie = new Movie { Id = movieId, Title = "A" };

                RepositoryMock
                    .Setup(repository => repository.CreateMovieAsync(It.IsAny<Entities.Movies.Movie>()))
                    .ReturnsAsync(0);

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
                var movieDetails = new MovieDetails
                {
                    Id = movieDetailsId,
                    OriginalTitle = "A"
                };

                RepositoryMock
                    .Setup(repository => repository.CreateMovieDetailsAsync(It.IsAny<Entities.Movies.MovieDetails>()))
                    .ReturnsAsync(new Func<Entities.Movies.MovieDetails, long>(m => m.Id));

                //Act
                var newId = await ServiceUnderTest.AddMovieDetailsAsync(movieDetails);

                movieDetailsId.Should().Be(newId);
            }

            [Fact]
            public void Should_throw_movie_details_not_added_exception_when_could_not_be_added()
            {
                //Arrange
                var movieDetailsId = 404L;
                var movieDetails = new MovieDetails { Id = movieDetailsId, OriginalTitle = "A" };

                RepositoryMock
                    .Setup(repository => repository.CreateMovieDetailsAsync(It.IsAny<Entities.Movies.MovieDetails>()))
                    .ReturnsAsync(0);
                //Act
                Func<Task<long>> action = () => ServiceUnderTest.AddMovieDetailsAsync(movieDetails);

                //Assert
                action.ShouldThrow<MovieDetailsNotAddedException>();
            }
        }
    }
}