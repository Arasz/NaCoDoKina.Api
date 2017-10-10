using System.Threading.Tasks;
using ApplicationCore.Entities.Cinemas;

namespace ApplicationCore.Repositories
{
    public interface ICinemaNetworkRepository
    {
        /// <summary>
        /// Creates cinema network 
        /// </summary>
        /// <param name="network"> New cinema network </param>
        /// <returns></returns>
        Task<long> CreateAsync(CinemaNetwork network);

        /// <summary>
        /// Gets cinema network by name 
        /// </summary>
        /// <param name="name"> Cinema network name </param>
        /// <returns></returns>
        Task<CinemaNetwork> GetByNameAsync(string name);

        /// <summary>
        /// Gets cinema network by id 
        /// </summary>
        /// <param name="id"> Cinema network id </param>
        /// <returns></returns>
        Task<CinemaNetwork> GetByIdAsync(long id);

        /// <summary>
        /// Checks if given cinema network exist in database 
        /// </summary>
        /// <param name="name"></param>
        /// <returns> True if exist </returns>
        Task<bool> ExistAsync(string name);
    }
}