using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Exceptions;
using NaCoDoKina.Api.Infrastructure.Recommendation.DataContract;
using NaCoDoKina.Api.Infrastructure.Recommendation.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Infrastructure.Recommendation.Services
{
    /// <inheritdoc/>
    /// <summary>
    /// Error handling decorator for recommendation service 
    /// </summary>
    public class RecommendationServiceErrorHandling : IRecommendationService
    {
        private readonly IRecommendationService _recommendationService;
        private readonly ILogger<IRecommendationService> _logger;

        public RecommendationServiceErrorHandling(IRecommendationService recommendationService, ILogger<IRecommendationService> logger)
        {
            _recommendationService = recommendationService ?? throw new ArgumentNullException(nameof(recommendationService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<RecommendationApiResponse> GetMovieRating(RecommendationApiRequest request)
        {
            try
            {
                return await _recommendationService.GetMovieRating(request);
            }
            catch (RecommendationApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogError("Not found movie or user in recommendation service" +
                                 " with request {@request}, exception {@e}", request, e);
                throw new MovieNotFoundException(request.MovieId);
            }
            catch (Exception unknownException)
            {
                _logger.LogError("Unknown error during recommendation api get request: " +
                                "{@unknownException}", unknownException);
                throw new RecommendationApiException(unknownException.Message);
            }
        }

        public async Task SaveMovieRating(RecommendationApiRequest request, Rating rating)
        {
            try
            {
                await _recommendationService.SaveMovieRating(request, rating);
            }
            catch (RecommendationApiException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogError("Not found movie or user in recommendation service" +
                                 " with request {@request}, exception {@e}", request, e);
                throw new MovieNotFoundException(request.MovieId);
            }
            catch (Exception unknownException)
            {
                _logger.LogError("Unknown error during recommendation api post request: " +
                                 "{@unknownException}", unknownException);
                throw new RecommendationApiException(unknownException.Message);
            }
        }
    }
}