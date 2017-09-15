using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.Entities;
using System;
using System.Collections.Generic;
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

        public Task<IEnumerable<Cinema>> GetAllCinemasForMovie(long movieId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Cinema>> GetAllCinemas()
        {
            throw new System.NotImplementedException();
        }

        public async Task<Cinema> AddCinema(Cinema cinema)
        {
            var entry = _applicationContext.Cinemas.Add(cinema);
            await _applicationContext.SaveChangesAsync();
            return entry.Entity;
        }
    }
}