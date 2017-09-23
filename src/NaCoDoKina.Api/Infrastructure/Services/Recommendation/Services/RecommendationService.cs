using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using NaCoDoKina.Api.Infrastructure.Services.Recommendation.DataContract;
using NaCoDoKina.Api.Infrastructure.Services.Recommendation.Exceptions;
using NaCoDoKina.Api.Infrastructure.Settings;

namespace NaCoDoKina.Api.Infrastructure.Services.Recommendation.Services
{
    public class RecommendationService : BaseHttpApiClient, IRecommendationService
    {
        private readonly RecommendationSettings _configuration;
        private readonly IRequestParser<RecommendationApiRequest> _requestParser;

        public RecommendationService(HttpClient httpClient, IRequestParser<RecommendationApiRequest> requestParser, RecommendationSettings configuration)
            : base(httpClient)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _requestParser = requestParser ?? throw new ArgumentNullException(nameof(requestParser));
        }

        public async Task<RecommendationApiResponse> GetMovieRating(RecommendationApiRequest request)
        {
            var parsedRequest = _requestParser.Parse(request);

            var url = CreateRequestUrl(parsedRequest);

            var response = await HttpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
                throw new RecommendationApiException(response.StatusCode, $"Recommendation api error {content}");

            return Deserialize<RecommendationApiResponse>(content);
        }

        public async Task SaveMovieRating(RecommendationApiRequest request, Rating rating)
        {
            var parsedRequest = _requestParser.Parse(request);

            var url = CreateRequestUrl(parsedRequest);

            var response = await HttpClient.PostAsync(url, new StringContent(Serialize(rating), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK)
                throw new RecommendationApiException(response.StatusCode, $"Recommendation api error {content}");
        }

        protected override string BaseUrl => _configuration.BaseUrl;
    }
}