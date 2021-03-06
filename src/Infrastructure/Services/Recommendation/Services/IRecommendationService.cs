﻿using System.Threading.Tasks;
using ApplicationCore.Results;
using Infrastructure.Services.Recommendation.DataContract;

namespace Infrastructure.Services.Recommendation.Services
{
    /// <summary>
    /// RegisterUser recommendation service 
    /// </summary>
    public interface IRecommendationService
    {
        /// <summary>
        /// Getting predicted or actual rating of the movie for the user 
        /// </summary>
        /// <param name="request"> Api request </param>
        /// <returns> Movie rating </returns>
        Task<Result<RecommendationApiResponse>> GetMovieRating(RecommendationApiRequest request);

        /// <summary>
        /// Crates or updates rating for user and movie 
        /// </summary>
        /// <param name="request"> Api request </param>
        /// <param name="rating"> Movie ratting </param>
        Task<Result> SaveMovieRating(RecommendationApiRequest request, Rating rating);
    }
}