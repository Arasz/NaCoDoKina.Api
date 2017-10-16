using Infrastructure.Services;
using Infrastructure.Services.Google.Services;
using Infrastructure.Settings;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net.Http;
using Infrastructure.Settings.Google;

namespace NaCoDoKina.Api.IntegrationTests.Infrastructure.Google
{
    public abstract class GoogleServiceTestBase<TService, TRequest>
    {
        protected TService ServiceUnderTest { get; }

        protected GoogleApiSettings ApiSettings { get; }

        protected abstract IRequestParser<TRequest> RequestParser { get; }

        protected abstract TService CreateServiceUnderTest(GoogleServiceDependencies<TRequest> dependencies);

        protected GoogleServiceTestBase()
        {
            ApiSettings = new GoogleApiSettings();
            var httpClient = new HttpClient();
            var logger = new NullLogger<BaseHttpApiClient>();
            var dependencies = new GoogleServiceDependencies<TRequest>(httpClient, RequestParser, logger, ApiSettings);
            ServiceUnderTest = CreateServiceUnderTest(dependencies);
        }
    }
}