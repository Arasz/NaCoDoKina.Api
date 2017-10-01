using Microsoft.EntityFrameworkCore;
using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.Entities.Cinemas;
using System;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Repositories
{
    public class CinemaNetworkRepository : ICinemaNetworkRepository
    {
        private readonly ApplicationContext _applicationContext;

        public CinemaNetworkRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext ?? throw new ArgumentNullException(nameof(applicationContext));
        }

        public async Task<long> CreateAsync(CinemaNetwork network)
        {
            var entityEntry = _applicationContext.CinemaNetworks.Add(network);
            await _applicationContext.SaveChangesAsync();
            return entityEntry.Entity.Id;
        }

        public Task<CinemaNetwork> GetByNameAsync(string name) => _applicationContext.CinemaNetworks
            .SingleOrDefaultAsync(network => network.Name == name);

        public Task<CinemaNetwork> GetByIdAsync(long id) => _applicationContext.CinemaNetworks
            .FindAsync(id);

        public async Task<bool> ExistAsync(string name) => await _applicationContext.CinemaNetworks
            .AnyAsync(network => network.Name == name);
    }
}