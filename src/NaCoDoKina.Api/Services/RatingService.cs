using NaCoDoKina.Api.Infrastructure.Recommendation.DataContract;
using NaCoDoKina.Api.Infrastructure.Recommendation.Services;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRecommendationService _recommendationService;
        private readonly IUserService _userService;

        public RatingService(IRecommendationService recommendationService, IUserService userService)
        {
            _recommendationService = recommendationService;
            _userService = userService;
        }

        public async Task<double> GetMovieRating(long movieId)
        {
            var userId = await _userService.GetCurrentUserIdAsync();
            var request = new RecommendationApiRequest(movieId, userId);
            var response = await _recommendationService.GetMovieRating(request);
            return response.Rating;
        }

        public async Task SetMovieRating(long movieId, double movieRating)
        {
            var userId = await _userService.GetCurrentUserIdAsync();
            var request = new RecommendationApiRequest(movieId, userId);
            var rating = new Rating(movieRating);
            await _recommendationService.SaveMovieRating(request, rating);
        }
    }
}