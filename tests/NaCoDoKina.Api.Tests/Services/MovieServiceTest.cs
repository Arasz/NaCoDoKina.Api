using Moq;
using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Repositories;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Services
{
    public class MovieServiceTest
    {
        protected MovieService ServiceUnderTest { get; }

        protected Mock<IRepository<Movie>> MovieRepositoryMock { get; }

        public MovieServiceTest()
        {
            MovieRepositoryMock = new Mock<IRepository<Movie>>();
            ServiceUnderTest = new MovieService(MovieRepositoryMock.Object);
        }
    }

    public class ReadAllMoviesAsync : MovieServiceTest
    {
        [Fact]
        public async Task Should_return_all_movies_available_for_user()
        {
            // Arrange
            var movies = new ReadOnlyCollection<Movie>(new List<Movie>
            {
                new Movie(),
                new Movie()
            });

            MovieRepositoryMock.Setup(repository => repository.ListAsync());

            // Act

            // Assert
        }
    }
}