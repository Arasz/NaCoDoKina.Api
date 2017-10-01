using NaCoDoKina.Api.Entities.Cinemas;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Repositories
{
    public interface ICinemaNetworkRepository
    {
        Task<long> CreateAsync(CinemaNetwork network);

        Task<CinemaNetwork> GetByNameAsync(string name);

        Task<CinemaNetwork> GetByIdAsync(long id);
    }
}