﻿using System;
using System.Threading.Tasks;
using ApplicationCore.Results;
using Infrastructure.Services.Recommendation.DataContract;
using Infrastructure.Services.Recommendation.Services;
using Infrastructure.Settings;

namespace Infrastructure.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRecommendationService _recommendationService;
        private readonly IUserService _userService;
        private readonly RatingSettings _ratingSettings;

        public RatingService(IRecommendationService recommendationService, IUserService userService, RatingSettings ratingSettings)
        {
            _recommendationService = recommendationService ?? throw new ArgumentNullException(nameof(recommendationService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _ratingSettings = ratingSettings ?? throw new ArgumentNullException(nameof(ratingSettings));
        }

        public async Task<double> GetMovieRating(long movieId)
        {
            var userId = _userService.GetCurrentUserId();
            var request = new RecommendationApiRequest(userId, movieId);

            var result = await _recommendationService.GetMovieRating(request);

            return result.IsSuccess ? result.Value.Rating : _ratingSettings.UnratedMovieRating;
        }

        public async Task<Result> SetMovieRating(long movieId, double movieRating)
        {
            var userId = _userService.GetCurrentUserId();
            var request = new RecommendationApiRequest(userId, movieId);
            var rating = new Rating(movieRating);
            return await _recommendationService.SaveMovieRating(request, rating);
        }
    }
}