using ApplicationCore.Entities.Cinemas;
using ApplicationCore.Entities.Movies;
using ApplicationCore.Repositories;
using CacheManager.Core;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MovieShowtimeRepository : IMovieShowtimeRepository
    {
        private readonly ICacheManager<MovieShowtime[]> _cacheManager;
        private readonly ApplicationContext _applicationContext;

        public MovieShowtimeRepository(ApplicationContext applicationContext, ICacheManager<MovieShowtime[]> cacheManager)
        {
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
        }

        public async Task<long> CreateMovieShowtimeAsync(MovieShowtime showtime)
        {
            _applicationContext.Cinemas.Attach(showtime.Cinema);
            _applicationContext.Movies.Attach(showtime.Movie);

            var entityEntry = _applicationContext.MovieShowtimes.Add(showtime);

            await _applicationContext.SaveChangesAsync();

            return entityEntry.Entity.Id;
        }

        public async Task CreateMovieShowtimesAsync(IEnumerable<MovieShowtime> showtimes)
        {
            foreach (var movieShowtime in showtimes)
            {
                var exist = await _applicationContext.MovieShowtimes
                    .Where(s => Equals(s.Cinema, movieShowtime.Cinema))
                    .Where(s => Equals(s.Movie, movieShowtime.Movie))
                    .Where(s => s.ShowTime == movieShowtime.ShowTime)
                    .AnyAsync();

                if (exist)
                    continue;

                _applicationContext.TryAttach(movieShowtime.Cinema);
                _applicationContext.TryAttach(movieShowtime.Movie);

                _applicationContext.MovieShowtimes.Add(movieShowtime);
            }

            await _applicationContext.SaveChangesAsync();
        }

        public async Task DeleteAllBeforeDateAsync(DateTime limitDateTime)
        {
            var showtimesToDelete = await _applicationContext.MovieShowtimes
                .Where(showtime => showtime.ShowTime < limitDateTime)
                .ToListAsync();

            _applicationContext.MovieShowtimes.RemoveRange(showtimesToDelete);
            await _applicationContext.SaveChangesAsync();

            _cacheManager.Clear();
        }

        public async Task<IEnumerable<MovieShowtime>> GetShowtimesForMovieAsync(long movieId, DateTime laterThan)
        {
            var cacheKey = $"{nameof(movieId)}{movieId}";
            var showtimes = _cacheManager.Get(cacheKey);

            if (showtimes is null || !showtimes.Any())
            {
                showtimes = await GetShowtimesWithMovieAndCinema()
                    .Where(showtime => showtime.Movie.Id == movieId)
                    .Where(showtime => showtime.ShowTime > laterThan)
                    .ToArrayAsync();

                _cacheManager.Put(cacheKey, showtimes);
                SetCacheExpireForShowtimes(cacheKey);
            }

            return showtimes;
        }

        private IIncludableQueryable<MovieShowtime, Cinema> GetShowtimesWithMovieAndCinema()
        {
            return _applicationContext.MovieShowtimes
                .Include(showtime => showtime.Movie)
                .Include(showtime => showtime.Cinema);
        }

        public async Task<IEnumerable<MovieShowtime>> GetShowtimesForCinemaAsync(long cinemaId, DateTime laterThan)
        {
            var cacheKey = $"{nameof(cinemaId)}{cinemaId}";
            var showtimes = _cacheManager.Get(cacheKey);

            if (showtimes is null || !showtimes.Any())
            {
                showtimes = await GetShowtimesWithMovieAndCinema()
                    .Where(showtime => showtime.Cinema.Id == cinemaId)
                    .Where(showtime => showtime.ShowTime > laterThan)
                    .ToArrayAsync();

                _cacheManager.Put(cacheKey, showtimes);
                SetCacheExpireForShowtimes(cacheKey);
            }

            return showtimes;
        }

        private void SetCacheExpireForShowtimes(string cacheKey)
        {
            _cacheManager.Expire(cacheKey, ExpirationMode.Absolute, TimeSpan.FromMinutes(5));
        }

        public async Task<IEnumerable<MovieShowtime>> GetShowtimesForCinemaAndMovieAsync(long movieId, long cinemaId, DateTime laterThan)
        {
            var cacheKey = $"{nameof(cinemaId)}{cinemaId}{nameof(movieId)}{movieId}";
            var showtimes = _cacheManager.Get(cacheKey);

            if (showtimes is null || !showtimes.Any())
            {
                showtimes = await GetShowtimesWithMovieAndCinema()
                    .Where(showtime => showtime.Cinema.Id == cinemaId)
                    .Where(showtime => showtime.Movie.Id == movieId)
                    .Where(showtime => showtime.ShowTime > laterThan)
                    .ToArrayAsync();

                _cacheManager.Put(cacheKey, showtimes);
                SetCacheExpireForShowtimes(cacheKey);
            }

            return showtimes;
        }
    }
}