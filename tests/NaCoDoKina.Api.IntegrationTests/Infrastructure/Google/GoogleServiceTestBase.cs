using IntegrationTestsCore;
using Microsoft.Extensions.DependencyInjection;

namespace NaCoDoKina.Api.IntegrationTests.Infrastructure.Google
{
    public abstract class GoogleServiceTestBase<TService> : HttpTestBase<Startup>
    {
        protected TService ServiceUnderTest { get; }

        protected GoogleServiceTestBase()
        {
            ServiceUnderTest = Services.GetService<TService>();
        }
    }
}