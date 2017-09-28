﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Repositories;
using Ploeh.AutoFixture;
using System;
using System.Linq;
using System.Threading.Tasks;
using NaCoDoKina.Api.Entities.Cinemas;
using Xunit;

namespace NaCoDoKina.Api.Repository
{
    public class MovieShowtimeRepositoryTest : ApplicationRepositoryTestBase<IMovieShowtimeRepository>
    {
        public class GetMovieShowtimesAsync : MovieShowtimeRepositoryTest
        {
            [Fact]
            public async Task Should_return_movie_showtime_when_movie_with_id_exist_and_is_later_than_now()
            {
                // Arrange
                var movieId = Fixture.Create<long>();
                var laterThan = DateTime.Now;
                var showtimeCount = 15;

                var showtimes = Fixture.Build<MovieShowtime>()
                    .With(showtime => showtime.ShowTime, laterThan.Add(TimeSpan.FromMinutes(1)))
                    .CreateMany(showtimeCount)
                    .ToArray();
                var selectedShowtime = showtimes.First();
                selectedShowtime.ShowTime = laterThan.AddMinutes(100);
                movieId = selectedShowtime.Movie.Id;

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.MovieShowtimes.AddRange(showtimes);
                    await scope.DbContext.SaveChangesAsync();
                }

                using (var scope = CreateContextScope())
                {
                    Mock.Provide(scope.DbContext);
                    RepositoryUnderTest = Mock.Create<MovieShowtimeRepository>();

                    // Act
                    var result = await RepositoryUnderTest.GetMovieShowtimesAsync(movieId, laterThan);

                    // Assert
                    result.Should().HaveCount(1);
                }

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.MovieShowtimes
                        .Count().Should().Be(showtimeCount);
                }
            }

            [Fact]
            public async Task Should_return_no_movie_showtime_when_movie_with_id_exist_and_earlier_than_now()
            {
                // Arrange
                var movieId = Fixture.Create<long>();
                var laterThan = DateTime.Now;
                var showtimeCount = 15;

                var showtimes = Fixture.Build<MovieShowtime>()
                    .With(showtime => showtime.ShowTime, laterThan.Add(TimeSpan.FromMinutes(1)))
                    .CreateMany(showtimeCount)
                    .ToArray();
                var selectedShowtime = showtimes.First();
                selectedShowtime.ShowTime = laterThan.AddMinutes(-100);
                movieId = selectedShowtime.Movie.Id;

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.MovieShowtimes.AddRange(showtimes);
                    await scope.DbContext.SaveChangesAsync();
                }

                using (var scope = CreateContextScope())
                {
                    Mock.Provide(scope.DbContext);
                    RepositoryUnderTest = Mock.Create<MovieShowtimeRepository>();

                    // Act
                    var result = await RepositoryUnderTest.GetMovieShowtimesAsync(movieId, laterThan);

                    // Assert
                    result.Should().HaveCount(0);
                }

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.MovieShowtimes
                        .Count().Should().Be(showtimeCount);
                }
            }

            [Fact]
            public async Task Should_return_no_movie_showtime_when_movie_with_id_do_not_exist()
            {
                // Arrange
                var movieId = Fixture.Create<long>();
                var laterThan = DateTime.Now;
                var showtimeCount = 15;

                var showtimes = Fixture.Build<MovieShowtime>()
                    .With(showtime => showtime.ShowTime, laterThan.Add(TimeSpan.FromMinutes(1)))
                    .CreateMany(showtimeCount)
                    .ToArray();

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.MovieShowtimes.AddRange(showtimes);
                    await scope.DbContext.SaveChangesAsync();
                }

                using (var scope = CreateContextScope())
                {
                    Mock.Provide(scope.DbContext);
                    RepositoryUnderTest = Mock.Create<MovieShowtimeRepository>();

                    // Act
                    var result = await RepositoryUnderTest.GetMovieShowtimesAsync(movieId, laterThan);

                    // Assert
                    result.Should().HaveCount(0);
                }

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.MovieShowtimes
                        .Count().Should().Be(showtimeCount);
                }
            }

            [Fact]
            public async Task Should_return_all_movie_showtimes_that_are_later_than_given_date()
            {
                // Arrange

                var laterThan = DateTime.Now;
                var showtimeCount = 15;

                var movie = Fixture.Create<Movie>();

                var laterShowtimes = Fixture.Build<MovieShowtime>()
                    .With(showtime => showtime.ShowTime, laterThan.Add(TimeSpan.FromMinutes(15)))
                    .With(showtime => showtime.Movie, movie)
                    .CreateMany(showtimeCount);

                var earlierShowtimes = Fixture.Build<MovieShowtime>()
                    .With(showtime => showtime.ShowTime, laterThan.Add(TimeSpan.FromMinutes(-15)))
                    .With(showtime => showtime.Movie, movie)
                    .CreateMany(showtimeCount);

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Movies.Add(movie);
                    scope.DbContext.MovieShowtimes.AddRange(laterShowtimes.Concat(earlierShowtimes));
                    await scope.DbContext.SaveChangesAsync();
                }

                using (var scope = CreateContextScope())
                {
                    Mock.Provide(scope.DbContext);
                    RepositoryUnderTest = Mock.Create<MovieShowtimeRepository>();

                    // Act
                    var result = await RepositoryUnderTest.GetMovieShowtimesAsync(movie.Id, laterThan);

                    // Assert
                    result.Should().HaveCount(showtimeCount);
                }

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.MovieShowtimes
                        .Count().Should().Be(2 * showtimeCount);
                }
            }
        }

        public class GetMovieShowtimesForCinemaAsync : MovieShowtimeRepositoryTest
        {
            [Fact]
            public async Task Should_return_movie_showtime_when_movie_and_cinema_with_id_exist_and_is_later_than_now()
            {
                // Arrange
                var movie = Fixture.Create<Movie>();
                var cinema = Fixture.Create<Cinema>();

                var laterThan = DateTime.Now;
                var showtimeCount = 15;

                var showtimes = Fixture.Build<MovieShowtime>()
                    .With(showtime => showtime.ShowTime, laterThan.Add(TimeSpan.FromMinutes(5)))
                    .CreateMany(showtimeCount)
                    .ToList();

                var foundShowtime = Fixture.Build<MovieShowtime>()
                    .With(showtime => showtime.ShowTime, laterThan.Add(TimeSpan.FromMinutes(22)))
                    .With(showtime => showtime.Movie, movie)
                    .With(showtime => showtime.Cinema, cinema)
                    .Create();

                showtimes.Add(foundShowtime);

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.MovieShowtimes.AddRange(showtimes);
                    await scope.DbContext.SaveChangesAsync();
                }

                using (var scope = CreateContextScope())
                {
                    Mock.Provide(scope.DbContext);
                    RepositoryUnderTest = Mock.Create<MovieShowtimeRepository>();

                    // Act
                    var result = await RepositoryUnderTest.GetMovieShowtimesForCinemaAsync(movie.Id, cinema.Id, laterThan);

                    // Assert
                    result.Should().HaveCount(1);
                    result.Should()
                        .ContainSingle(showtime => showtime.Id == foundShowtime.Id);
                }

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.MovieShowtimes
                        .Count().Should().Be(showtimeCount + 1);
                }
            }

            [Fact]
            public async Task Should_return_no_movie_showtime_when_movie_with_id_exist_and_is_earlier_than_now()
            {
                // Arrange
                var movie = Fixture.Create<Movie>();
                var cinema = Fixture.Create<Cinema>();

                var laterThan = DateTime.Now;
                var showtimeCount = 15;

                var showtimes = Fixture.Build<MovieShowtime>()
                    .With(showtime => showtime.ShowTime, laterThan.Add(TimeSpan.FromMinutes(5)))
                    .CreateMany(showtimeCount)
                    .ToList();

                var foundShowtime = Fixture.Build<MovieShowtime>()
                    .With(showtime => showtime.ShowTime, laterThan.Add(TimeSpan.FromMinutes(-20)))
                    .With(showtime => showtime.Movie, movie)
                    .With(showtime => showtime.Cinema, cinema)
                    .Create();

                showtimes.Add(foundShowtime);

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.MovieShowtimes.AddRange(showtimes);
                    await scope.DbContext.SaveChangesAsync();
                }

                using (var scope = CreateContextScope())
                {
                    Mock.Provide(scope.DbContext);
                    RepositoryUnderTest = Mock.Create<MovieShowtimeRepository>();

                    // Act
                    var result = await RepositoryUnderTest.GetMovieShowtimesForCinemaAsync(movie.Id, cinema.Id, laterThan);

                    // Assert
                    result.Should().HaveCount(0);
                }

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.MovieShowtimes
                        .Count().Should().Be(showtimeCount + 1);
                }
            }

            [Fact]
            public async Task Should_return_no_movie_showtime_when_movie_with_id_do_not_exist()
            {
                // Arrange
                var movie = Fixture.Create<Movie>();
                var cinema = Fixture.Create<Cinema>();

                var laterThan = DateTime.Now;
                var showtimeCount = 15;

                var showtimes = Fixture.Build<MovieShowtime>()
                    .With(showtime => showtime.ShowTime, laterThan.Add(TimeSpan.FromMinutes(5)))
                    .CreateMany(showtimeCount)
                    .ToList();

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.MovieShowtimes.AddRange(showtimes);
                    await scope.DbContext.SaveChangesAsync();
                }

                using (var scope = CreateContextScope())
                {
                    Mock.Provide(scope.DbContext);
                    RepositoryUnderTest = Mock.Create<MovieShowtimeRepository>();

                    // Act
                    var result = await RepositoryUnderTest.GetMovieShowtimesForCinemaAsync(movie.Id, cinema.Id, laterThan);

                    // Assert
                    result.Should().HaveCount(0);
                }

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.MovieShowtimes
                        .Count().Should().Be(showtimeCount);
                }
            }

            [Fact]
            public async Task Should_return_all_movie_showtimes_that_are_later_than_given_date()
            {
                // Arrange
                var movie = Fixture.Create<Movie>();
                var cinema = Fixture.Create<Cinema>();

                var laterThan = DateTime.Now;
                var showtimeCount = 15;

                var laterShowtimes = Fixture.Build<MovieShowtime>()
                    .With(showtime => showtime.ShowTime, laterThan.Add(TimeSpan.FromMinutes(5)))
                    .With(showtime => showtime.Movie, movie)
                    .With(showtime => showtime.Cinema, cinema)
                    .CreateMany(showtimeCount);

                var earlierShowtimes = Fixture.Build<MovieShowtime>()
                    .With(showtime => showtime.ShowTime, laterThan.Add(TimeSpan.FromMinutes(-5)))
                    .With(showtime => showtime.Movie, movie)
                    .With(showtime => showtime.Cinema, cinema)
                    .CreateMany(showtimeCount);

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.Movies.Add(movie);
                    scope.DbContext.Cinemas.Add(cinema);
                    scope.DbContext.MovieShowtimes.AddRange(laterShowtimes.Concat(earlierShowtimes));
                    await scope.DbContext.SaveChangesAsync();
                }

                using (var scope = CreateContextScope())
                {
                    Mock.Provide(scope.DbContext);
                    RepositoryUnderTest = Mock.Create<MovieShowtimeRepository>();

                    // Act
                    var result = await RepositoryUnderTest.GetMovieShowtimesForCinemaAsync(movie.Id, cinema.Id, laterThan);

                    // Assert
                    result.Should().HaveCount(showtimeCount);
                }

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.MovieShowtimes
                        .Count().Should().Be(2 * showtimeCount);
                }
            }
        }

        public class DeleteAllBeforeDateAsync : MovieShowtimeRepositoryTest
        {
            [Fact]
            public async Task Should_delete_all_before_given_date()
            {
                // Arrange

                var deleteDate = DateTime.Now.AddMinutes(20);
                var showtimeCount = 15;

                var notDeleted = Fixture.Build<MovieShowtime>()
                    .With(showtime => showtime.ShowTime, DateTime.Now.AddMinutes(100))
                    .Create();

                var showtimes = Fixture.Build<MovieShowtime>()
                    .With(showtime => showtime.ShowTime, DateTime.Now.AddMinutes(1))
                    .CreateMany(showtimeCount)
                    .Append(notDeleted);

                using (var scope = CreateContextScope())
                {
                    scope.DbContext.MovieShowtimes.AddRange(showtimes);
                    await scope.DbContext.SaveChangesAsync();
                }

                using (var scope = CreateContextScope())
                {
                    Mock.Provide(scope.DbContext);
                    RepositoryUnderTest = Mock.Create<MovieShowtimeRepository>();

                    // Act
                    await RepositoryUnderTest.DeleteAllBeforeDateAsync(deleteDate);
                }

                // Assert
                using (var scope = CreateContextScope())
                {
                    scope.DbContext.MovieShowtimes
                        .Count().Should().Be(1);
                    var single = await scope.DbContext.MovieShowtimes
                        .SingleAsync();
                    single.Id.Should().Be(notDeleted.Id);
                }
            }
        }

        public class AddMovieShowtimeAsync : MovieShowtimeRepositoryTest
        {
            [Fact]
            public async Task Should_add_showtime_and_return_id()
            {
                //Arrange
                var showtimeId = 1;

                var movie = new Movie
                {
                    Name = nameof(Movie),
                    Details = new MovieDetails(),
                    PosterUrl = nameof(Movie.PosterUrl),
                };

                var cinema = new Cinema
                {
                    Name = nameof(Cinema),
                    Address = nameof(Cinema.Address),
                    Location = new Location(1, 1)
                };

                var movieShowtime = new MovieShowtime
                {
                    Cinema = cinema,
                    Movie = movie,
                    ShowTime = DateTime.Now.AddHours(2)
                };

                using (var contextScope = CreateContextScope())
                {
                    cinema.Id = contextScope.DbContext.Cinemas.Add(cinema).Entity.Id;
                    movie.Id = contextScope.DbContext.Movies.Add(movie).Entity.Id;

                    await contextScope.DbContext.SaveChangesAsync();
                }

                using (var contextScope = CreateContextScope())
                {
                    RepositoryUnderTest = new MovieShowtimeRepository(contextScope.DbContext, LoggerMock.Object);

                    //Act
                    var movieShowtimeId = await RepositoryUnderTest.AddMovieShowtimeAsync(movieShowtime);

                    //Assert
                    movieShowtimeId.Should().BePositive();
                }

                using (var contextScope = CreateContextScope())
                {
                    contextScope.DbContext.MovieShowtimes
                        .Should().HaveCount(1);

                    contextScope.DbContext.MovieShowtimes
                        .Should().ContainSingle(showtime => showtime.Id == showtimeId);
                }
            }
        }
    }
}