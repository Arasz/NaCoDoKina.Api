using ApplicationCore.Entities.Cinemas;
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
    public class CinemaRepository : ICinemaRepository
    {
        private readonly ICacheManager<Cinema> _cinemaCacheManager;
        private readonly ICacheManager<Cinema[]> _cacheManager;
        private readonly ApplicationContext _applicationContext;

        public CinemaRepository(ApplicationContext applicationContext, ICacheManager<Cinema[]> cinemasCache, ICacheManager<Cinema> cinemaCache)
        {
            _cinemaCacheManager = cinemaCache ?? throw new ArgumentNullException(nameof(cinemaCache));
            _cacheManager = cinemasCache ?? throw new ArgumentNullException(nameof(cinemasCache));
            _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
        }

        public async Task<IEnumerable<Cinema>> GetAllCinemasForMovieAsync(long movieId)
        {
            var cinemaIds = _applicationContext.MovieShowtimes
                .Where(showtime => showtime.ShowTime > DateTime.Now)
                .Where(showtime => showtime.Movie.Id == movieId)
                .Select(showtime => showtime.Cinema.Id)
                .Distinct()
                .ToHashSet();

            var cinemasForMovie = await _applicationContext.Cinemas
                .Include(cinema => cinema.CinemaNetwork)
                .Where(cinema => cinemaIds.Contains(cinema.Id))
                .ToListAsync();

            return cinemasForMovie.AsEnumerable();
        }

        public async Task<IEnumerable<Cinema>> GetAllCinemas()
        {
            var allCinemas = await _applicationContext.Cinemas
                .Include(cinema => cinema.CinemaNetwork)
                .ToListAsync();
            return allCinemas.AsEnumerable();
        }

        public async Task CreateCinemasAsync(IEnumerable<Cinema> cinemas)
        {
            _applicationContext.Cinemas.AddRange(cinemas);

            await _applicationContext.SaveChangesAsync();
        }

        public async Task<Cinema> CreateCinemaAsync(Cinema cinema)
        {
            var entry = _applicationContext.Cinemas.Add(cinema);
            await _applicationContext.SaveChangesAsync();
            return entry.Entity;
        }

        public Task<Cinema> GetCinemaByIdAsync(long id)
        {
            return _applicationContext.Cinemas.FindAsync(id);
        }

        public Task<Cinema> GetCinemaByNameAsync(string name)
        {
            return _applicationContext.Cinemas
                .SingleOrDefaultAsync(c => c.Name == name);
        }
    }
}