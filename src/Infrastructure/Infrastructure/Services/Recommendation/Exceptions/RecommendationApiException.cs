using System.Net;
using NaCoDoKina.Api.Exceptions;

namespace NaCoDoKina.Api.Infrastructure.Services.Recommendation.Exceptions
{
    public class RecommendationApiException : NaCoDoKinaApiException
    {
        public HttpStatusCode StatusCode { get; }

        public RecommendationApiException(HttpStatusCode statusCode, string userName) : base(userName)
        {
            StatusCode = statusCode;
        }

        public RecommendationApiException(string userName) : base(userName)
        {
        }
    }
}