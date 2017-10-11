using ApplicationCore.Entities.Movies;
using ApplicationCore.Repositories;
using CacheManager.Core;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ICacheManager<MovieDetails> _movieDetailsCacheManager;
        private readonly IMovieShowtimeRepository _movieShowtimeRepository;
        private readonly ICacheManager<Movie> _movieCacheManager;
        private readonly ApplicationContext _applicationContext;

        public MovieRepository(ApplicationContext applicationContext, IMovieShowtimeRepository movieShowtimeRepository,
            ICacheManager<Movie> movieCacheManager, ICacheManager<MovieDetails> movieDetailsCacheManager)
        {
            _movieDetailsCacheManager = movieDetailsCacheManager ?? throw new ArgumentNullException(nameof(movieDetailsCacheManager));
            _movieShowtimeRepository = movieShowtimeRepository ?? throw new ArgumentNullException(nameof(movieShowtimeRepository));
            _movieCacheManager = movieCacheManager ?? throw new ArgumentNullException(nameof(movieCacheManager));
            _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
        }

        public async Task<bool> DeleteMovieAsync(long movieId)
        {
            var movieToDelete = await _applicationContext.Movies.FindAsync(movieId);

            if (movieToDelete is null)
                return false;

            var movieDetails = await _applicationContext.MovieDetails
                .SingleAsync(details => details.Id == movieId);

            var movieShowtimes = _applicationContext.MovieShowtimes
                .Where(showtime => showtime.Movie.Id == movieId);

            _applicationContext.RemoveRange(movieShowtimes);

            _applicationContext.Remove(movieToDelete);
            _applicationContext.Remove(movieDetails);

            var deletedMovies = _applicationContext.DisabledMovies
                .Where(mark => mark.MovieId == movieId);

            _applicationContext.RemoveRange(deletedMovies);

            await _applicationContext.SaveChangesAsync();

            var stringId = movieId.ToString();
            _movieCacheManager.Remove(stringId);
            _movieDetailsCacheManager.Remove(stringId);

            return true;
        }

        public async Task<Movie> GetMovieAsync(long id)
        {
            var movie = _movieCacheManager.Get(id.ToString());

            if (movie is null)
            {
                movie = await _applicationContext.Movies
                    .FindAsync(id);

                if (movie is null)
                    return default(Movie);

                _movieCacheManager.Put(id.ToString(), movie);
            }

            return movie;
        }

        public async Task<Movie> GetMovieByExternalIdAsync(string externalId)
        {
            var movie = _movieCacheManager.Get(externalId);
            if (movie is null)
            {
                movie = await _applicationContext.Movies
                    .Include(m => m.ExternalMovies)
                    .Where(m => m.ExternalMovies.Any(externalMovie => externalMovie.ExternalId == externalId))
                    .SingleOrDefaultAsync();

                if (movie is null)
                    return default(Movie);

                _movieCacheManager.Put(externalId, movie);
            }

            return movie;
        }

        public async Task<IEnumerable<long>> GetMoviesForCinemaAsync(long cinemaId, DateTime laterThan)
        {
            var showtimes = await _movieShowtimeRepository
                .GetShowtimesForCinemaAsync(cinemaId, laterThan);

            var movies = showtimes
                .Select(showtime => showtime.Movie.Id);

            return movies;
        }

        public async Task<MovieDetails> GetMovieDetailsAsync(long id)
        {
            var movieDetails = _movieDetailsCacheManager.Get(id.ToString());

            if (movieDetails is null)
            {
                movieDetails = await _applicationContext.MovieDetails
                    .Include(details => details.MediaResources)
                    .Include(details => details.MovieReviews)
                    .SingleOrDefaultAsync(details => details.Id == id);

                if (movieDetails is null)
                    return default(MovieDetails);

                _movieDetailsCacheManager.Put(id.ToString(), movieDetails);
            }

            return movieDetails;
        }

        public async Task CreateMoviesAsync(IEnumerable<Movie> movies)
        {
            _applicationContext.AddRange(movies);
            await _applicationContext.SaveChangesAsync();
        }

        public async Task<long> CreateMovieAsync(Movie newMovie)
        {
            var entityEntry = _applicationContext.Movies.Add(newMovie);
            await _applicationContext.SaveChangesAsync();
            return entityEntry.Entity.Id;
        }

        public async Task<long> CreateMovieDetailsAsync(MovieDetails movieDetails)
        {
            var movie = await _applicationContext.Movies.FindAsync(movieDetails.Id);

            if (movie is null)
                return default(long);

            movieDetails.Id = movie.Id;

            _applicationContext.MovieDetails.Update(movieDetails);

            movie.Details = movieDetails;

            _applicationContext.Movies.Update(movie);

            await _applicationContext.SaveChangesAsync();

            return movie.Id;
        }
    }
}