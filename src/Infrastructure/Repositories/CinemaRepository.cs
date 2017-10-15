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
        private readonly ICacheManager<ICollection<Cinema>> _cinemasCacheManager;
        private readonly ApplicationContext _applicationContext;

        public CinemaRepository(ApplicationContext applicationContext, ICacheManager<ICollection<Cinema>> cinemasCache, ICacheManager<Cinema> cinemaCache)
        {
            _cinemaCacheManager = cinemaCache ?? throw new ArgumentNullException(nameof(cinemaCache));
            _cinemasCacheManager = cinemasCache ?? throw new ArgumentNullException(nameof(cinemasCache));
            _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
        }

        public async Task<ICollection<Cinema>> GetCinemasByCityAsync(string city)
        {
            var cinemas = _cinemasCacheManager.Get(city);

            if (cinemas is null)
            {
                cinemas = await _applicationContext.Cinemas
                    .Where(cinema => cinema.Address.EndsWith(city))
                    .ToArrayAsync();

                _cinemasCacheManager.Put(city, cinemas);
            }

            return cinemas;
        }

        public async Task<ICollection<Cinema>> GetAllCinemasForMovieAsync(long movieId)
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
                .ToArrayAsync();

            return cinemasForMovie;
        }

        public async Task<ICollection<Cinema>> GetAllCinemas()
        {
            var cinemas = _cinemasCacheManager.Get(nameof(GetAllCinemas));

            if (cinemas is null)
            {
                cinemas = await _applicationContext.Cinemas
                    .Include(cinema => cinema.CinemaNetwork)
                    .ToArrayAsync();

                _cinemasCacheManager.Put(nameof(GetAllCinemas), cinemas);
            }

            return cinemas;
        }

        public async Task CreateCinemasAsync(IEnumerable<Cinema> cinemas)
        {
            foreach (var cinema in cinemas)
            {
                var exist = await _applicationContext.Cinemas
                    .Where(c => c.ExternalId == cinema.ExternalId || c.Name == cinema.Name)
                    .AnyAsync();

                if (exist)
                    continue;

                _applicationContext.Cinemas.Add(cinema);
            }

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