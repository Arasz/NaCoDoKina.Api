using NaCoDoKina.Api.Repositories;

namespace NaCoDoKina.Api.Repository
{
    public class MovieRepositoryTest : RepositoryTestBase<IMovieRepository>
    {
        public MovieRepositoryTest()
        {
            RepositoryUnderTest = new MovieRepository();
        }
    }
}