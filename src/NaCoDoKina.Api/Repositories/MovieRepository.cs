using NaCoDoKina.Api.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        public Task<bool> DeleteMovieAsync(long movieId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteMovieAsync(long movieId, long userId)
        {
            throw new NotImplementedException();
        }

        public Task<Movie> GetMovieAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<long>> GetMoviesPlayedInCinemaAsync(long cinemaId, DateTime laterThan)
        {
            throw new NotImplementedException();
        }

        public Task<MovieDetails> GetMovieDetailsAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<long> AddMovieAsync(Movie newMovie)
        {
            throw new NotImplementedException();
        }

        public Task<long> AddMovieDetailsAsync(MovieDetails movieDetails)
        {
            throw new NotImplementedException();
        }
    }
}