using NaCoDoKina.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Location = NaCoDoKina.Api.DataContracts.Location;
using MovieDetails = NaCoDoKina.Api.DataContracts.MovieDetails;

namespace NaCoDoKina.Api.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _repository;

        public MovieService(IMovieRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Task<IEnumerable<long>> GetAllMoviesAsync(Location location)
        {
            throw new System.NotImplementedException();
        }

        public Task<DataContracts.Movie> GetMovieAsync(long id)
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