using System.Net.Http;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Configuration;

namespace NaCoDoKina.Api.Infrastructure.Services.Google.Services
{
    public class GoogleServiceDependencies<TRequest>
    {
        public GoogleServiceDependencies(HttpClient httpClient, IRequestParser<TRequest> requestParser, ILogger<BaseHttpApiClient> logger, GoogleApiConfiguration configuration)
        {
            HttpClient = httpClient;
            RequestParser = requestParser;
            Logger = logger;
            ApiKey = configuration.ApiKey;
        }

        public HttpClient HttpClient { get; }

        public IRequestParser<TRequest> RequestParser { get; }

        public ILogger<BaseHttpApiClient> Logger { get; }

        public string ApiKey { get; }
    }
}