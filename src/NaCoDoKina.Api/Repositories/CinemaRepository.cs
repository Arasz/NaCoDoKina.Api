using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.Entities.Cinemas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Repositories
{
    public class CinemaRepository : ICinemaRepository
    {
        private readonly ApplicationContext _applicationContext;
        private readonly ILogger<ICinemaRepository> _logger;

        public CinemaRepository(ApplicationContext applicationContext, ILogger<ICinemaRepository> logger)
        {
            _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                .Include(cinema => cinema.Website)
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