using NaCoDoKina.Api.Models;
using System;
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

        public Task<IEnumerable<long>> GetAllMoviesAsync(SearchArea searchArea)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<long>> GetMoviesPlayedInCinemas(IEnumerable<Cinema> cinemas, TimeSpan arrivalTime)
        {
            throw new NotImplementedException();
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

        public Task<long> AddMovieAsync(Movie newMovie)
        {
            throw new NotImplementedException();
        }

        public Task<long> AddMovieDetails(long movieId, MovieDetails movieDetails)
        {
            throw new NotImplementedException();
        }
    }
}