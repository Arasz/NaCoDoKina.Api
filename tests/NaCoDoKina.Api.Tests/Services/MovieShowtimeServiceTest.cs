using FluentAssertions;
using Moq;
using NaCoDoKina.Api.Exceptions;
using NaCoDoKina.Api.Models;
using NaCoDoKina.Api.Repositories;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Services
{
    public class MovieShowtimeServiceTest : ServiceWithRepositoryTestBase<IMovieShowtimeService, IMovieShowtimeRepository>
    {
        public MovieShowtimeServiceTest()
        {
            ServiceUnderTest = new MovieShowtimeService(RepositoryMockObject, LoggerMock.Object, MapperMock.Object);
        }

        protected void InitializeMapping()
        {
            MapperMock
                .Setup(mapper => mapper.Map<MovieShowtime>(It.IsAny<Entities.MovieShowtime>()))
                .Returns(new Func<Entities.MovieShowtime, MovieShowtime>(showtime => new MovieShowtime
                {
                    MovieId = showtime.Movie.Id,
                    CinemaId = showtime.Cinema.Id,
                    Language = showtime.Language,
                    ShowTime = showtime.ShowTime,
                    ShowType = showtime.ShowType,
                }));
        }

        public class GetMovieShowtimesForCinemaAsync : MovieShowtimeServiceTest
        {
            [Fact]
            public async Task Should_return_movie_showtimes_when_given_existing_movie_id_and_cinema_id()
            {
                //Arrange

                var movieId = Fixture.Create<long>();
                var cinemaId = Fixture.Create<long>();
                var laterThan = Fixture.Create<DateTime>();
                Fixture.Customize<Entities.Movie>(composer =>
                {
                    return composer.With(movie => movie.Id, movieId);
                });
                var movieShowtimes = Fixture
                    .CreateMany<Entities.MovieShowtime>()
                    .ToArray();

                RepositoryMock
                    .Setup(repository => repository.GetMovieShowtimesForCinemaAsync(movieId, cinemaId, laterThan))
                    .ReturnsAsync(movieShowtimes);

                InitializeMapping();

                // Act
                var showtimes = await ServiceUnderTest.GetMovieShowtimesForCinemaAsync(movieId, cinemaId, laterThan);

                // Assert
                showtimes.Should()
                    .NotBeNullOrEmpty()
                    .And
                    .HaveSameCount(movieShowtimes)
                    .And
                    .Match(enumerable => enumerable.All(showtime => showtime.MovieId == movieId));
            }

            [Fact]
            public void Should_throw_MovieShowtimeNotFoundException_when_movie_dont_have_shows()
            {
                //Arrange

                var movieId = Fixture.Create<long>();
                var cinemaId = Fixture.Create<long>();
                var laterThan = Fixture.Create<DateTime>();
                var movieShowtimes = Fixture.CreateMany<Entities.MovieShowtime>(0);

                RepositoryMock
                    .Setup(repository => repository.GetMovieShowtimesForCinemaAsync(movieId, cinemaId, laterThan))
                    .ReturnsAsync(movieShowtimes);

                InitializeMapping();

                // Act
                Func<Task<IEnumerable<MovieShowtime>>> action =
                    () => ServiceUnderTest.GetMovieShowtimesForCinemaAsync(movieId, cinemaId, laterThan);

                // Assert
                action.ShouldThrow<MovieShowtimeNotFoundException>();
            }
        }

        public class GetMovieShowtimesAsync : MovieShowtimeServiceTest
        {
            [Fact]
            public async Task Should_return_movie_showtimes_when_given_existing_movie_id()
            {
                //Arrange

                var movieId = Fixture.Create<long>();
                var laterThan = Fixture.Create<DateTime>();
                Fixture.Customize<Entities.Movie>(composer =>
                {
                    return composer.With(movie => movie.Id, movieId);
                });
                var movieShowtimes = Fixture
                    .CreateMany<Entities.MovieShowtime>()
                    .ToArray();

                RepositoryMock
                    .Setup(repository => repository.GetMovieShowtimesAsync(movieId, laterThan))
                    .ReturnsAsync(movieShowtimes);

                InitializeMapping();

                // Act
                var showtimes = await ServiceUnderTest.GetMovieShowtimesAsync(movieId, laterThan);

                // Assert
                showtimes.Should()
                    .NotBeNullOrEmpty()
                    .And
                    .HaveSameCount(movieShowtimes)
                    .And
                    .Match(enumerable => enumerable.All(showtime => showtime.MovieId == movieId));
            }

            [Fact]
            public void Should_throw_MovieShowtimeNotFoundException_when_movie_dont_have_shows()
            {
                //Arrange

                var movieId = Fixture.Create<long>();
                var cinemaId = Fixture.Create<long>();
                var laterThan = Fixture.Create<DateTime>();
                var movieShowtimes = Fixture.CreateMany<Entities.MovieShowtime>(0);

                RepositoryMock
                    .Setup(repository => repository.GetMovieShowtimesAsync(movieId, laterThan))
                    .ReturnsAsync(movieShowtimes);

                InitializeMapping();

                // Act
                Func<Task<IEnumerable<MovieShowtime>>> action = () =>
                    ServiceUnderTest.GetMovieShowtimesForCinemaAsync(movieId, cinemaId, laterThan);

                // Assert
                action.ShouldThrow<MovieShowtimeNotFoundException>();
            }
        }
    }
}