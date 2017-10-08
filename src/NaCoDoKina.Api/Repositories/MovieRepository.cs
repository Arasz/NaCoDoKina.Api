using CacheManager.Core;
using Microsoft.EntityFrameworkCore;
using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.Entities.Movies;
using NaCoDoKina.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ICacheManager<Movie> _movieCacheManager;
        private readonly IUserService _userService;
        private readonly ApplicationContext _applicationContext;

        public MovieRepository(ApplicationContext applicationContext, IUserService userService, ICacheManager<Movie> movieCacheManager)
        {
            _movieCacheManager = movieCacheManager ?? throw new ArgumentNullException(nameof(movieCacheManager));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
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

            var deletedMovies = _applicationContext.DeletedMovies
                .Where(mark => mark.MovieId == movieId);

            _applicationContext.RemoveRange(deletedMovies);

            await _applicationContext.SaveChangesAsync();

            _movieCacheManager.Remove(movieId.ToString());

            return true;
        }

        public async Task<bool> SoftDeleteMovieAsync(long movieId)
        {
            var userId = _userService.GetCurrentUserId();

            var movieExist = await _applicationContext.Movies.AnyAsync(movie => movie.Id == movieId);

            if (!movieExist)
                return false;

            var markExist = await _applicationContext.DeletedMovies
                .Where(mark => mark.UserId == userId)
                .AnyAsync(mark => mark.MovieId == movieId);

            if (markExist)
                return true;

            _applicationContext.DeletedMovies.Add(new DeletedMovies(movieId, userId));
            await _applicationContext.SaveChangesAsync();

            return true;
        }

        private async Task<bool> IsMovieSoftDeletedAsync(long id)
        {
            var userId = _userService.GetCurrentUserId();

            return await _applicationContext.DeletedMovies
                .Where(mark => mark.MovieId == id)
                .Where(mark => mark.UserId == userId)
                .AnyAsync();
        }

        public async Task<Movie> GetMovieAsync(long id)
        {
            var isMovieSoftDeleted = await IsMovieSoftDeletedAsync(id);
            if (isMovieSoftDeleted)
                return default(Movie);

            var movie = _movieCacheManager.Get(id.ToString());

            if (movie is null)
            {
                movie = await _applicationContext.Movies
                    .FindAsync(id);

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

                _movieCacheManager.Put(externalId, movie);
            }

            return movie;
        }

        public async Task<IEnumerable<long>> GetMoviesIdsPlayedInCinemaAsync(long cinemaId, DateTime laterThan)
        {
            var userId = _userService.GetCurrentUserId();

            var markedAsDeletedMoviesIds = _applicationContext.DeletedMovies
                .Where(mark => mark.UserId == userId)
                .Select(mark => mark.MovieId)
                .Distinct()
                .ToHashSet();

            var allMoviesPlayedInCinema = await _applicationContext.MovieShowtimes
                 .Where(showtime => showtime.ShowTime > laterThan)
                 .Where(showtime => showtime.Cinema.Id == cinemaId)
                 .Select(showtime => showtime.Movie.Id)
                 .Except(markedAsDeletedMoviesIds)
                 .ToArrayAsync();

            return allMoviesPlayedInCinema;
        }

        public async Task<MovieDetails> GetMovieDetailsAsync(long id)
        {
            var isMovieSoftDeleted = await IsMovieSoftDeletedAsync(id);
            if (isMovieSoftDeleted)
                return default(MovieDetails);

            return await _applicationContext.MovieDetails
                .Include(details => details.MediaResources)
                .Include(details => details.MovieReviews)
                .SingleOrDefaultAsync(details => details.Id == id);
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