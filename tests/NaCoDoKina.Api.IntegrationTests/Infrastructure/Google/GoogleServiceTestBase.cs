using Microsoft.Extensions.Logging.Abstractions;
using NaCoDoKina.Api.Infrastructure;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common.Request;
using NaCoDoKina.Api.Infrastructure.Google.Services;
using System.Net.Http;

namespace NaCoDoKina.Api.IntegrationTests.Infrastructure.Google
{
    public abstract class GoogleServiceTestBase<TService, TRequest>
    {
        protected TService ServiceUnderTest { get; }

        protected const string ApiKey = "AIzaSyB0k9n49t5OXZ9XUfh8n9zUfhmdQ-_Tt5M";

        protected abstract IRequestParser<TRequest> RequestParser { get; }

        protected abstract TService CreateServiceUnderTest(GoogleServiceDependencies<TRequest> dependencies);

        protected GoogleServiceTestBase()
        {
            var httpClient = new HttpClient();
            var logger = new NullLogger<BaseHttpApiClient>();
            var dependencies = new GoogleServiceDependencies<TRequest>(httpClient, RequestParser, logger, ApiKey);
            ServiceUnderTest = CreateServiceUnderTest(dependencies);
        }
    }
}