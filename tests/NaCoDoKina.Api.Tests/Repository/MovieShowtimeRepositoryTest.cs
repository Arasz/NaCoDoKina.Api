using FluentAssertions;
using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Repository
{
    public class MovieShowtimeRepositoryTest : RepositoryTestBase<IMovieShowtimeRepository>
    {
        public class DeleteAllBeforeDateAsync : MovieShowtimeRepositoryTest
        {
        }

        public class AddMovieShowtimeAsync : MovieShowtimeRepositoryTest
        {
            [Fact]
            public async Task Should_add_showtime_and_return_id()
            {
                using (var databaseScope = new InMemoryDatabaseScope())
                {
                    EnsureCreated(databaseScope);

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

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        cinema.Id = contextScope.ApplicationContext.Cinemas.Add(cinema).Entity.Id;
                        movie.Id = contextScope.ApplicationContext.Movies.Add(movie).Entity.Id;

                        await contextScope.ApplicationContext.SaveChangesAsync();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        RepositoryUnderTest = new MovieShowtimeRepository(contextScope.ApplicationContext, LoggerMock.Object);

                        //Act
                        var movieShowtimeId = await RepositoryUnderTest.AddMovieShowtimeAsync(movieShowtime);

                        //Assert
                        movieShowtimeId.Should().BePositive();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.MovieShowtimes
                            .Should().HaveCount(1);

                        contextScope.ApplicationContext.MovieShowtimes
                            .Should().ContainSingle(showtime => showtime.Id == showtimeId);
                    }
                }
            }
        }
    }
}