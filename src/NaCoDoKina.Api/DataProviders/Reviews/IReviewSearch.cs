using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.Reviews
{
    /// <summary>
    /// Searches for given entity web page in some external service 
    /// </summary>
    public interface IReviewSearch<in TEntity>
    {
        /// <summary>
        /// Searches for web page address connected with entity in external service 
        /// </summary>
        /// <param name="entity"> Entity searched for </param>
        /// <returns> Web page url connected with given entity </returns>
        Task<string> Search(TEntity entity);
    }
}