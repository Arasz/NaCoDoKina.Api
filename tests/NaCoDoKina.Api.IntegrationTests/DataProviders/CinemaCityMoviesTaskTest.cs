using FluentAssertions;
using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.DataProviders.CinemaCity.Movies;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.IntegrationTests.DataProviders
{
    public class CinemaCityMoviesTaskTest : TaskTestBase<CinemaCityMoviesTask>
    {
        [Fact]
        public async Task Should_execute_task_and_save_all_movies_from_cinema_city_to_database()
        {
            // Arrange
            var context = GetDbContext<ApplicationContext>();

            var moviesCount = context.Movies.Count();

            // Act

            await TaskUnderTest.Execute();

            // Assert

            var newCount = context.Movies.Count();

            newCount.Should().BeGreaterThan(moviesCount);
        }
    }
}