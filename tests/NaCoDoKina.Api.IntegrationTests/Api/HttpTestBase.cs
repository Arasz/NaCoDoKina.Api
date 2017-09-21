using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net.Http;

namespace NaCoDoKina.Api.IntegrationTests.Api
{
    /// <inheritdoc/>
    /// <summary>
    /// Base class for all integration test using in memory test server implementation 
    /// </summary>
    public abstract class HttpTestBase : IDisposable
    {
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            Server?.Dispose();
            Client?.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected TestServer Server { get; }

        protected HttpClient Client { get; }

        protected virtual Uri BaseAddress => new Uri(@"http://localhost:5000");

        protected virtual string Environment => "Development";

        protected HttpTestBase()
        {
            var builder = WebHost.CreateDefaultBuilder()
                .UseEnvironment(Environment)
                .UseContentRoot(GetContentRoot())
                .ConfigureServices(ConfigureServices)
                .UseStartup<Startup>();

            Server = new TestServer(builder);

            Client = Server.CreateClient();
            Client.BaseAddress = BaseAddress;
        }

        private string GetContentRoot()
        {
            var baseDirectory = new DirectoryInfo(AppContext.BaseDirectory);

            while (baseDirectory.Name != "NaCoDoKinaApi")
            {
                baseDirectory = baseDirectory.Parent;
            }

            return Path.Combine(baseDirectory.FullName, "src", "NaCoDoKina.Api");
        }

        protected virtual void ConfigureServices(WebHostBuilderContext webHostBuilderContext, IServiceCollection serviceCollection)
        {
        }
    }
}