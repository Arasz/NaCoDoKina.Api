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
            var cinemasForMovie = await _applicationContext.MovieShowtimes
                .Include(showtime => showtime.Cinema)
                    .ThenInclude(cinema => cinema.Website)
                .Include(showtime => showtime.Cinema)
                    .ThenInclude(cinema => cinema.CinemaNetwork)
                .Include(showtime => showtime.Movie)
                .Where(showtime => showtime.ShowTime > DateTime.Now)
                .Where(showtime => showtime.Movie.Id == movieId)
                .Select(showtime => showtime.Cinema)
                .Distinct()
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

        public async Task<Cinema> AddCinema(Cinema cinema)
        {
            var entry = _applicationContext.Cinemas.Add(cinema);
            await _applicationContext.SaveChangesAsync();
            return entry.Entity;
        }

        public Task<Cinema> GetCinemaAsync(long id)
        {
            return _applicationContext.Cinemas.FindAsync(id);
        }

        public Task<Cinema> GetCinemaAsync(string name)
        {
            return _applicationContext.Cinemas
                .SingleOrDefaultAsync(c => c.Name == name);
        }
    }
}