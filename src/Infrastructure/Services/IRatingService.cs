using System.Threading.Tasks;
using ApplicationCore.Results;

namespace Infrastructure.Services
{
    /// <summary>
    /// Business logic for movies rating 
    /// </summary>
    public interface IRatingService
    {
        /// <summary>
        /// Gets rating for given movie 
        /// </summary>
        /// <param name="movieId"> Movie id </param>
        /// <returns> Movie rating </returns>
        Task<double> GetMovieRating(long movieId);

        /// <summary>
        /// Sets rating for movie 
        /// </summary>
        /// <param name="movieId"> Movie id </param>
        /// <param name="movieRating"> Movie rating </param>
        Task<Result> SetMovieRating(long movieId, double movieRating);
    }
}