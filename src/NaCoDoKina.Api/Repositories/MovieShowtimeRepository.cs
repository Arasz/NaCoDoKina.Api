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
    public class MovieShowtimeRepository : IMovieShowtimeRepository
    {
        private readonly ApplicationContext _applicationContext;
        private readonly ILogger<IMovieShowtimeRepository> _logger;

        public MovieShowtimeRepository(ApplicationContext applicationContext, ILogger<IMovieShowtimeRepository> logger)
        {
            _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<long> AddMovieShowtimeAsync(MovieShowtime showtime)
        {
            _applicationContext.Cinemas.Attach(showtime.Cinema);
            _applicationContext.Movies.Attach(showtime.Movie);

            var entityEntry = _applicationContext.MovieShowtimes.Add(showtime);

            await _applicationContext.SaveChangesAsync();

            return entityEntry.Entity.Id;
        }

        public async Task DeleteAllBeforeDateAsync(DateTime limitDateTime)
        {
            var showtimesToDelete = await _applicationContext.MovieShowtimes
                .Where(showtime => showtime.ShowTime < limitDateTime)
                .ToListAsync();

            _applicationContext.MovieShowtimes.RemoveRange(showtimesToDelete);
        }

        public async Task<IEnumerable<MovieShowtime>> GetMovieShowtimesAsync(long movieId, DateTime laterThan)
        {
            var showtimes = await _applicationContext.MovieShowtimes
                 .Include(showtime => showtime.Movie)
                 .Include(showtime => showtime.Cinema)
                 .Where(showtime => showtime.Movie.Id == movieId)
                 .Where(showtime => showtime.ShowTime > laterThan)
                 .ToListAsync();

            return showtimes;
        }

        public async Task<IEnumerable<MovieShowtime>> GetMovieShowtimesForCinemaAsync(long movieId, long cinemaId, DateTime laterThan)
        {
            var showtimes = await _applicationContext.MovieShowtimes
                .Include(showtime => showtime.Movie)
                .Include(showtime => showtime.Cinema)
                .Where(showtime => showtime.Cinema.Id == cinemaId)
                .Where(showtime => showtime.Movie.Id == movieId)
                .Where(showtime => showtime.ShowTime > laterThan)
                .ToListAsync();

            return showtimes;
        }
    }
}