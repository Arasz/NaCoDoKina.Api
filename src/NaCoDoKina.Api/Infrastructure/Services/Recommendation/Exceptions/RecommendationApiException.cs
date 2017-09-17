using System.Net;
using NaCoDoKina.Api.Exceptions;

namespace NaCoDoKina.Api.Infrastructure.Services.Recommendation.Exceptions
{
    public class RecommendationApiException : NaCoDoKinaApiException
    {
        public HttpStatusCode StatusCode { get; }

        public RecommendationApiException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        public RecommendationApiException(string message) : base(message)
        {
        }
    }
}