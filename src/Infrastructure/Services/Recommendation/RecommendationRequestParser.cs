﻿using Infrastructure.Services.Recommendation.DataContract;

namespace Infrastructure.Services.Recommendation
{
    /// <inheritdoc/>
    /// <summary>
    /// Recommendation api request parser 
    /// </summary>
    public class RecommendationRequestParser : IRequestParser<RecommendationApiRequest>
    {
        public string Parse(RecommendationApiRequest request)
        {
            return $"/v1/ratings/movies/{request.MovieId}/users/{request.UserId}";
        }
    }
}