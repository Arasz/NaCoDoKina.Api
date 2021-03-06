﻿using ApplicationCore.Repositories;
using FluentAssertions;
using Infrastructure.Exceptions;
using Infrastructure.Models.Movies;
using Infrastructure.Services;
using Moq;
using Ploeh.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Movie = ApplicationCore.Entities.Movies.Movie;

namespace NaCoDoKina.Api.Services
{
    public class MovieShowtimeServiceTest : ServiceWithRepositoryTestBase<MovieShowtimeService, IMovieShowtimeRepository>
    {
        protected void InitializeMapping()
        {
            MapperMock
                .Setup(mapper => mapper.Map<MovieShowtime>(It.IsAny<ApplicationCore.Entities.Movies.MovieShowtime>()))
                .Returns(new Func<ApplicationCore.Entities.Movies.MovieShowtime, MovieShowtime>(showtime => new MovieShowtime
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
                Fixture.Customize<Movie>(composer =>
                {
                    return composer.With(movie => movie.Id, movieId);
                });
                var movieShowtimes = Fixture
                    .CreateMany<ApplicationCore.Entities.Movies.MovieShowtime>()
                    .ToArray();

                RepositoryMock
                    .Setup(repository => repository.GetShowtimesForCinemaAndMovieAsync(movieId, cinemaId, laterThan))
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
                var movieShowtimes = Fixture.CreateMany<ApplicationCore.Entities.Movies.MovieShowtime>(0);

                RepositoryMock
                    .Setup(repository => repository.GetShowtimesForCinemaAndMovieAsync(movieId, cinemaId, laterThan))
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
                Fixture.Customize<Movie>(composer =>
                {
                    return composer.With(movie => movie.Id, movieId);
                });
                var movieShowtimes = Fixture
                    .CreateMany<ApplicationCore.Entities.Movies.MovieShowtime>()
                    .ToArray();

                RepositoryMock
                    .Setup(repository => repository.GetShowtimesForMovieAsync(movieId, laterThan))
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
                var movieShowtimes = Fixture.CreateMany<ApplicationCore.Entities.Movies.MovieShowtime>(0);

                RepositoryMock
                    .Setup(repository => repository.GetShowtimesForMovieAsync(movieId, laterThan))
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