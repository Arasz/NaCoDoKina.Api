using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Infrastructure.Services.Recommendation.DataContract;
using NaCoDoKina.Api.Infrastructure.Services.Recommendation.Exceptions;
using NaCoDoKina.Api.Infrastructure.Settings;
using NaCoDoKina.Api.Results;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Infrastructure.Services.Recommendation.Services
{
    public class RecommendationService : BaseHttpApiClient, IRecommendationService
    {
        private readonly ILogger<IRecommendationService> _logger;
        private readonly RecommendationSettings _configuration;
        private readonly IRequestParser<RecommendationApiRequest> _requestParser;

        public RecommendationService(HttpClient httpClient, IRequestParser<RecommendationApiRequest> requestParser, RecommendationSettings configuration, ILogger<IRecommendationService> logger)
            : base(httpClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _requestParser = requestParser ?? throw new ArgumentNullException(nameof(requestParser));
        }

        public async Task<Result<RecommendationApiResponse>> GetMovieRating(RecommendationApiRequest request)
        {
            try
            {
                var parsedRequest = _requestParser.Parse(request);

                var url = CreateRequestUrl(parsedRequest);

                var response = await HttpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return Result.Success(Deserialize<RecommendationApiResponse>(content));

                    case HttpStatusCode.NotFound:
                        _logger.LogError("Not found movie or user in recommendation service with request {@request}, response {@response} and message content {@content}", request, response, content);
                        return Result.Failure<RecommendationApiResponse>("Movie or user not found");

                    case HttpStatusCode.InternalServerError:
                        return Result.Failure<RecommendationApiResponse>("No model available");

                    default:
                        _logger.LogWarning("$Unexpected result code from recommendation api {code} in response {@response} with {@content}", response.StatusCode, response, content);
                        return Result.Failure<RecommendationApiResponse>("Unexpected result");
                }
            }
            catch (Exception unknownException)
            {
                _logger.LogError("Unknown error during {methodName} with request {@request} {@unknownException}", nameof(GetMovieRating), request, unknownException);
                throw new RecommendationApiException(unknownException.Message);
            }
        }

        public async Task<Result> SaveMovieRating(RecommendationApiRequest request, Rating rating)
        {
            try
            {
                var parsedRequest = _requestParser.Parse(request);

                var url = CreateRequestUrl(parsedRequest);

                var response = await HttpClient.PostAsync(url, new StringContent(Serialize(rating), Encoding.UTF8, "application/json"));
                var content = await response.Content.ReadAsStringAsync();

                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return Result.Success();

                    default:
                        _logger.LogWarning("$Unexpected result code from recommendation api {code} in response {@response} with {@content}", response.StatusCode, response, content);
                        return Result.Failure("Saving movie rating failed");
                }
            }
            catch (Exception unknownException)
            {
                _logger.LogError("Unknown error during {methodName} with request {@request} {@unknownException}", nameof(GetMovieRating), request, unknownException);
                throw new RecommendationApiException(unknownException.Message);
            }
        }

        protected override string BaseUrl => _configuration.BaseUrl;
    }
}