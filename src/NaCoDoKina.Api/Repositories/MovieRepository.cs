using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationContext _applicationContext;
        private readonly ILogger<IMovieRepository> _logger;

        public MovieRepository(ApplicationContext applicationContext, ILogger<IMovieRepository> logger)
        {
            _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> DeleteMovieAsync(long movieId)
        {
            var toDelete = await _applicationContext.Movies.FindAsync(movieId);

            if (toDelete is null)
                return false;

            _applicationContext.Movies.Remove(toDelete);

            return true;
        }

        public async Task<bool> SoftDeleteMovieAsync(long movieId, long userId)
        {
            var movieExist = await _applicationContext.Movies.AnyAsync(movie => movie.Id == movieId);

            if (!movieExist)
                return false;

            var markExist = await _applicationContext.DeletedMovieMarks
                .Where(mark => mark.UserId == userId)
                .AnyAsync(mark => mark.MovieId == movieId);

            if (markExist)
                return true;

            _applicationContext.DeletedMovieMarks.Add(new DeletedMovieMark(movieId, userId));
            await _applicationContext.SaveChangesAsync();

            return true;
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