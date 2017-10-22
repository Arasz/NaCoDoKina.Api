using ApplicationCore.Entities.Movies;
using ApplicationCore.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ExternalMovieRepository : IExternalMovieRepository

    {
        private readonly ApplicationContext _applicationContext;

        public ExternalMovieRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<IEnumerable<ExternalMovie>> GetExternalMoviesByExternalIdAsync(string externalId)
        {
            return await _applicationContext.Set<ExternalMovie>()
                .Where(em => em.ExternalId == externalId)
                .ToArrayAsync();
        }
    }
}