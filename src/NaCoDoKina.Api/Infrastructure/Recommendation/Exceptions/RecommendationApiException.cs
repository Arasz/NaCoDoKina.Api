using NaCoDoKina.Api.Exceptions;
using System.Net;

namespace NaCoDoKina.Api.Infrastructure.Recommendation.Exceptions
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