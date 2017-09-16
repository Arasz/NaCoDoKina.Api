using FluentAssertions;
using Moq;
using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Repositories;
using NaCoDoKina.Api.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Repository
{
    public class MovieRepositoryTest : RepositoryTestBase<IMovieRepository>
    {
        public Mock<IUserService> UserServiceMock { get; }

        public long DefaultUserId { get; } = 55;

        public MovieRepositoryTest()
        {
            UserServiceMock = new Mock<IUserService>();

            UserServiceMock
                .Setup(service => service.GetCurrentUserIdAsync())
                .Returns(() => Task.FromResult(DefaultUserId));
        }

        public class AddMovieAsync : MovieRepositoryTest
        {
            [Fact]
            public async Task Should_add_movie_and_return_id()
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
                        RepositoryUnderTest = new MovieRepository(contextScope.ApplicationContext, UserServiceMock.Object, LoggerMock.Object);

                        //Act
                        var addedMovieId = await RepositoryUnderTest.AddMovieAsync(movie);

                        //Assert
                        addedMovieId.Should().Be(movieId);
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Movies
                            .Any(m => m.Id == movieId)
                            .Should().BeTrue();
                    }
                }
            }
        }

        public class AddMovieDetailsAsync : MovieRepositoryTest
        {
            [Fact]
            public async Task Should_add_movie_details_and_return_id()
            {
                using (var databaseScope = new InMemoryDatabaseScope())
                {
                    EnsureCreated(databaseScope);

                    //Arrange
                    var movieId = 1;

                    var movie = new Movie
                    {
                        Name = nameof(Movie),
                        Details = new MovieDetails
                        {
                            Description = nameof(MovieDetails)
                        },
                        PosterUrl = nameof(Movie.PosterUrl),
                    };

                    var movieDetails = new MovieDetails
                    {
                        Id = 0,
                        MovieId = movieId,
                        Description = nameof(MovieDetails.Description),
                    };

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Movies.Add(movie);
                        await contextScope.ApplicationContext.SaveChangesAsync();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        RepositoryUnderTest = new MovieRepository(contextScope.ApplicationContext, UserServiceMock.Object, LoggerMock.Object);

                        //Act
                        var addedDetailsId = await RepositoryUnderTest.AddMovieDetailsAsync(movieDetails);

                        //Assert
                        addedDetailsId.Should().Be(movieId);
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Movies
                            .Any(m => m.Id == movieId)
                            .Should().BeTrue();

                        contextScope.ApplicationContext.MovieDetails
                            .Any(details => details.Id == movieId)
                            .Should().BeTrue();

                        contextScope.ApplicationContext.MovieDetails
                            .Single().Description.Should().Be(nameof(MovieDetails.Description));
                    }
                }
            }

            [Fact]
            public async Task Should_return_0_id_when_movie_can_not_be_found()
            {
                using (var databaseScope = new InMemoryDatabaseScope())
                {
                    EnsureCreated(databaseScope);

                    //Arrange
                    var movieId = 1;
                    var notExistingMovieId = 404L;

                    var movie = new Movie
                    {
                        Name = nameof(Movie),
                        Details = new MovieDetails
                        {
                            Description = nameof(MovieDetails)
                        },
                        PosterUrl = nameof(Movie.PosterUrl),
                    };

                    var movieDetails = new MovieDetails
                    {
                        Id = 0,
                        MovieId = notExistingMovieId,
                        Description = nameof(MovieDetails.Description),
                    };

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Movies.Add(movie);
                        await contextScope.ApplicationContext.SaveChangesAsync();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        RepositoryUnderTest = new MovieRepository(contextScope.ApplicationContext, UserServiceMock.Object, LoggerMock.Object);

                        //Act
                        var addedDetailsId = await RepositoryUnderTest.AddMovieDetailsAsync(movieDetails);

                        //Assert
                        addedDetailsId.Should().Be(0);
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Movies
                            .Any(m => m.Id == movieId)
                            .Should().BeTrue();

                        contextScope.ApplicationContext.MovieDetails
                            .Should().OnlyContain(details => details.Description == nameof(MovieDetails));
                    }
                }
            }
        }

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
                        RepositoryUnderTest = new MovieRepository(contextScope.ApplicationContext, UserServiceMock.Object, LoggerMock.Object);

                        //Act
                        var deleted = await RepositoryUnderTest.SoftDeleteMovieAsync(movieId);

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
                            .Should().ContainSingle(mark => mark.MovieId == movieId && mark.UserId == DefaultUserId);
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
                        RepositoryUnderTest = new MovieRepository(contextScope.ApplicationContext, UserServiceMock.Object, LoggerMock.Object);

                        //Act
                        var deleted = await RepositoryUnderTest.SoftDeleteMovieAsync(nonExistingId);

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

                    var movie = new Movie
                    {
                        Name = nameof(Movie),
                        Details = new MovieDetails(),
                        PosterUrl = nameof(Movie.PosterUrl),
                    };

                    var deletedMovieMark = new DeletedMovieMark(movieId, DefaultUserId);

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Movies.Add(movie);
                        contextScope.ApplicationContext.DeletedMovieMarks.Add(deletedMovieMark);
                        await contextScope.ApplicationContext.SaveChangesAsync();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        RepositoryUnderTest = new MovieRepository(contextScope.ApplicationContext, UserServiceMock.Object, LoggerMock.Object);

                        //Act
                        var deleted = await RepositoryUnderTest.SoftDeleteMovieAsync(movieId);

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
                            .ContainSingle(mark => mark.MovieId == deletedMovieMark.MovieId && mark.UserId == DefaultUserId);
                    }
                }
            }
        }

        public class GetMovieAsync : MovieRepositoryTest
        {
            [Fact]
            public async Task Should_return_movie_when_exist_and_is_not_marked_as_deleted()
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
                        RepositoryUnderTest = new MovieRepository(contextScope.ApplicationContext, UserServiceMock.Object, LoggerMock.Object);

                        //Act
                        var movieFromDb = await RepositoryUnderTest.GetMovieAsync(movieId);

                        //Assert
                        movieFromDb.Id.Should().BePositive();
                        movieFromDb.Details.Should().BeNull("We only get necessary data");
                        movieFromDb.Name.Should().Be(movie.Name);
                    }
                }
            }

            [Fact]
            public async Task Should_return_null_when_movie_do_not_exist()
            {
                using (var databaseScope = new InMemoryDatabaseScope())
                {
                    EnsureCreated(databaseScope);

                    //Arrange
                    var nonExistingMovieId = 52;

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
                        RepositoryUnderTest = new MovieRepository(contextScope.ApplicationContext, UserServiceMock.Object, LoggerMock.Object);

                        //Act
                        var movieFromDb = await RepositoryUnderTest.GetMovieAsync(nonExistingMovieId);

                        //Assert
                        movieFromDb.Should().BeNull();
                    }
                }
            }

            [Fact]
            public async Task Should_return_null_when_movie_is_soft_deleted()
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

                    var deletedMovieMark = new DeletedMovieMark(movieId, DefaultUserId);

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Movies.Add(movie);
                        contextScope.ApplicationContext.DeletedMovieMarks.Add(deletedMovieMark);
                        await contextScope.ApplicationContext.SaveChangesAsync();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        RepositoryUnderTest = new MovieRepository(contextScope.ApplicationContext, UserServiceMock.Object, LoggerMock.Object);

                        //Act
                        var movieFromDb = await RepositoryUnderTest.GetMovieAsync(movieId);

                        //Assert
                        movieFromDb.Should().BeNull();
                    }
                }
            }
        }

        public class GetMovieDetailsAsync : MovieRepositoryTest
        {
            [Fact]
            public async Task Should_return_movie_details_when_exist_and_is_not_marked_as_deleted()
            {
                using (var databaseScope = new InMemoryDatabaseScope())
                {
                    EnsureCreated(databaseScope);

                    //Arrange
                    var movieId = 1;

                    var movie = new Movie
                    {
                        Name = nameof(Movie),
                        Details = new MovieDetails
                        {
                            ReleaseDate = DateTime.MinValue,
                            Description = nameof(MovieDetails)
                        },
                        PosterUrl = nameof(Movie.PosterUrl),
                    };

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Movies.Add(movie);

                        await contextScope.ApplicationContext.SaveChangesAsync();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        RepositoryUnderTest = new MovieRepository(contextScope.ApplicationContext, UserServiceMock.Object, LoggerMock.Object);

                        //Act
                        var movieFromDb = await RepositoryUnderTest.GetMovieDetailsAsync(movieId);

                        //Assert
                        movieFromDb.Id.Should().BePositive();
                        movieFromDb.ReleaseDate.Should().Be(DateTime.MinValue);
                    }
                }
            }

            [Fact]
            public async Task Should_return_null_when_movie_details_do_not_exist()
            {
                using (var databaseScope = new InMemoryDatabaseScope())
                {
                    EnsureCreated(databaseScope);

                    //Arrange
                    var nonExistingMovieId = 52;

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
                        RepositoryUnderTest = new MovieRepository(contextScope.ApplicationContext, UserServiceMock.Object, LoggerMock.Object);

                        //Act
                        var movieFromDb = await RepositoryUnderTest.GetMovieDetailsAsync(nonExistingMovieId);

                        //Assert
                        movieFromDb.Should().BeNull();
                    }
                }
            }

            [Fact]
            public async Task Should_return_null_when_movie_is_soft_deleted()
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

                    var deletedMovieMark = new DeletedMovieMark(movieId, DefaultUserId);

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Movies.Add(movie);
                        contextScope.ApplicationContext.DeletedMovieMarks.Add(deletedMovieMark);
                        await contextScope.ApplicationContext.SaveChangesAsync();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        RepositoryUnderTest = new MovieRepository(contextScope.ApplicationContext, UserServiceMock.Object, LoggerMock.Object);

                        //Act
                        var movieFromDb = await RepositoryUnderTest.GetMovieDetailsAsync(movieId);

                        //Assert
                        movieFromDb.Should().BeNull();
                    }
                }
            }
        }

        public class GetMoviesPlayedInCinemaAsync : MovieRepositoryTest
        {
            [Fact]
            public async Task Should_return_all_not_deleted_movies_ids_played_in_cinema()
            {
                using (var databaseScope = new InMemoryDatabaseScope())
                {
                    EnsureCreated(databaseScope);

                    //Arrange
                    var movieId = 1;
                    var deletedMovieId = 2;
                    var cinemaId = 1;

                    var movie = new Movie
                    {
                        Name = nameof(Movie),
                        Details = new MovieDetails(),
                        PosterUrl = nameof(Movie.PosterUrl),
                    };

                    var deletedMovie = new Movie
                    {
                        Name = "Deleted" + nameof(Movie),
                        Details = new MovieDetails(),
                        PosterUrl = nameof(Movie.PosterUrl),
                    };

                    var deleteMovieMark = new DeletedMovieMark(deletedMovieId, DefaultUserId);

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
                        contextScope.ApplicationContext.Cinemas.Add(cinema);
                        contextScope.ApplicationContext.MovieShowtimes.Add(movieShowtime);
                        contextScope.ApplicationContext.Movies.Add(movie);
                        contextScope.ApplicationContext.Movies.Add(deletedMovie);
                        contextScope.ApplicationContext.DeletedMovieMarks.Add(deleteMovieMark);

                        await contextScope.ApplicationContext.SaveChangesAsync();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        RepositoryUnderTest = new MovieRepository(contextScope.ApplicationContext, UserServiceMock.Object, LoggerMock.Object);

                        //Act
                        var playedMoviesIds = await RepositoryUnderTest.GetMoviesIdsPlayedInCinemaAsync(cinemaId, DateTime.Now);

                        //Assert
                        playedMoviesIds.Should().HaveCount(1);
                        playedMoviesIds.Single().Should().Be(movieId);
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Movies
                            .Should().HaveCount(2);
                    }
                }
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

                    var movieDeletedMark = new DeletedMovieMark(movieId, DefaultUserId);

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        contextScope.ApplicationContext.Movies.Add(movie);
                        contextScope.ApplicationContext.DeletedMovieMarks.Add(movieDeletedMark);

                        await contextScope.ApplicationContext.SaveChangesAsync();
                    }

                    using (var contextScope = new TestDatabaseContextScope(databaseScope))
                    {
                        RepositoryUnderTest = new MovieRepository(contextScope.ApplicationContext, UserServiceMock.Object, LoggerMock.Object);

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
                        contextScope.ApplicationContext.DeletedMovieMarks
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
                        RepositoryUnderTest = new MovieRepository(contextScope.ApplicationContext, UserServiceMock.Object, LoggerMock.Object);

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