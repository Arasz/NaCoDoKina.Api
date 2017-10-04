using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.Parsers
{
    /// <summary>
    /// Parses html url to given entity class 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IWebPageMapper<in TEntity>
    {
        /// <summary>
        /// Maps web page content to TEntity object 
        /// </summary>
        /// <param name="url"> Web page url </param>
        /// <param name="mapped"> Object to fill </param>
        Task MapAsync(string url, TEntity mapped);
    }
}