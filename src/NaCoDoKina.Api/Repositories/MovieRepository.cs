using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly IUserService _userService;
        private readonly ApplicationContext _applicationContext;
        private readonly ILogger<IMovieRepository> _logger;

        public MovieRepository(ApplicationContext applicationContext, IUserService userService, ILogger<IMovieRepository> logger)
        {
            _userService = userService;
            _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> DeleteMovieAsync(long movieId)
        {
            var movieToDelete = await _applicationContext.Movies.FindAsync(movieId);

            if (movieToDelete is null)
                return false;

            var detailsToDelete = await _applicationContext.MovieDetails.FindAsync(movieId);

            _applicationContext.Movies.Remove(movieToDelete);
            _applicationContext.MovieDetails.Remove(detailsToDelete);

            var softDeletesForMovie = _applicationContext.DeletedMovieMarks
                .Where(mark => mark.MovieId == movieId);

            _applicationContext.DeletedMovieMarks.RemoveRange(softDeletesForMovie);

            await _applicationContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteMovieAsync(long movieId)
        {
            var userId = await _userService.GetCurrentUserIdAsync();

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

        private async Task<bool> IsMovieSoftDeletedAsync(long id)
        {
            var userId = await _userService.GetCurrentUserIdAsync();

            return await _applicationContext.DeletedMovieMarks
                .Where(mark => mark.MovieId == id)
                .Where(mark => mark.UserId == userId)
                .AnyAsync();
        }

        public async Task<Movie> GetMovieAsync(long id)
        {
            var isMovieSoftDeleted = await IsMovieSoftDeletedAsync(id);
            if (isMovieSoftDeleted)
                return default(Movie);

            return await _applicationContext.Movies
                .FindAsync(id);
        }

        public async Task<IEnumerable<long>> GetMoviesIdsPlayedInCinemaAsync(long cinemaId, DateTime laterThan)
        {
            var userId = await _userService.GetCurrentUserIdAsync();

            var markedAsDeletedMoviesIds = _applicationContext.DeletedMovieMarks
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

            return await _applicationContext.MovieDetails.FindAsync(id);
        }

        public async Task<long> AddMovieAsync(Movie newMovie)
        {
            var entityEntry = _applicationContext.Movies.Add(newMovie);
            await _applicationContext.SaveChangesAsync();
            return entityEntry.Entity.Id;
        }

        public async Task<long> AddMovieDetailsAsync(MovieDetails movieDetails)
        {
            var movie = await _applicationContext.Movies.FindAsync(movieDetails.MovieId);

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