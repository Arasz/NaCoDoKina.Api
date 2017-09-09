using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common;
using System.Net.Http;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common.Request;

namespace NaCoDoKina.Api.Infrastructure.Google.Services
{
    public class GoogleServiceDependencies<TRequest>
    {
        public GoogleServiceDependencies(HttpClient httpClient, IRequestParser<TRequest> requestParser, ILogger<BaseHttpApiClient> logger, string apiKey)
        {
            HttpClient = httpClient;
            RequestParser = requestParser;
            Logger = logger;
            ApiKey = apiKey;
        }

        public HttpClient HttpClient { get; }

        public IRequestParser<TRequest> RequestParser { get; }

        public ILogger<BaseHttpApiClient> Logger { get; }

        public string ApiKey { get; }
    }
}