using ApplicationCore.Entities.Movies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Repositories
{
    public interface IExternalMovieRepository
    {
        /// <summary>
        /// Returns collection of external movies with the same external id 
        /// </summary>
        /// <param name="externalId"> Id in external service </param>
        /// <returns></returns>
        Task<IEnumerable<ExternalMovie>> GetExternalMoviesByExternalIdAsync(string externalId);
    }
}