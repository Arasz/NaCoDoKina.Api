using Moq;
using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Repositories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace NaCoDoKina.Api.Services
{
    public class MovieServiceTest : ServiceWithRepositoryTestBase<IMovieService, IMovieRepository>
    {
        public MovieServiceTest()
        {
            RepositoryMock = new Mock<IMovieRepository>();
            ServiceUnderTest = new MovieService();
        }

        public class ReadAllMoviesAsync : MovieServiceTest
        {
            [Fact]
            public void Should_return_all_movies_available_for_user()
            {
                // Arrange
                var movies = new ReadOnlyCollection<Movie>(new List<Movie>
                {
                    new Movie(),
                    new Movie()
                });

                // Act

                // Assert
            }
        }
    }
}