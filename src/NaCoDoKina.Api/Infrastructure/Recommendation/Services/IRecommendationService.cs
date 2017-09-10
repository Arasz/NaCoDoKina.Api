using NaCoDoKina.Api.Infrastructure.Recommendation.DataContract;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Infrastructure.Recommendation.Services
{
    /// <summary>
    /// User recommendation service 
    /// </summary>
    public interface IRecommendationService
    {
        /// <summary>
        /// Getting predicted rating of the movie for the user 
        /// </summary>
        /// <param name="request"> Api request </param>
        /// <returns> Movie rating </returns>
        Task<RecommendationApiResponse> GetMovieRating(RecommendationApiRequest request);

        /// <summary>
        /// Saves user rating 
        /// </summary>
        /// <param name="request"> Api request </param>
        /// <param name="rating"> Movie ratting </param>
        Task SaveMovieRating(RecommendationApiRequest request, Rating rating);
    }
}