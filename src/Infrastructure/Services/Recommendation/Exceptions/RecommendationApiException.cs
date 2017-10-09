using System.Net;
using Infrastructure.Exceptions;

namespace Infrastructure.Services.Recommendation.Exceptions
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