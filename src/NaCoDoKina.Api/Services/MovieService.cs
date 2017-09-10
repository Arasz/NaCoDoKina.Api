using NaCoDoKina.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Services
{
    public class MovieService : IMovieService
    {
        public Task<IEnumerable<long>> GetAllMoviesAsync(Location location)
        {
            throw new System.NotImplementedException();
        }

        public Task<Movie> GetMovieAsync(long id)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteMovieAsync(long id)
        {
            throw new System.NotImplementedException();
        }

        public Task<MovieDetails> GetMovieDetailsAsync(long id)
        {
            throw new System.NotImplementedException();
        }
    }
}