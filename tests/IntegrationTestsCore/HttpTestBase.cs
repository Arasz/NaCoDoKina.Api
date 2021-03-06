﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Ploeh.AutoFixture;
using Serilog;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using TestsCore;

namespace IntegrationTestsCore
{
    /// <inheritdoc/>
    /// <summary>
    /// Base class for all integration test using in memory test server implementation 
    /// </summary>
    public abstract class HttpTestBase<TStartup> : UnitTestBase
        where TStartup : class
    {
        protected IntegrationTestsSettings TestsSettings { get; set; }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing)
                return;

            Server?.Dispose();
            Client?.Dispose();
        }

        protected TestServer Server { get; }

        protected HttpClient Client { get; }

        public IServiceProvider Services { get; }

        protected HttpTestBase()
        {
            LoadTestConfiguration();

            var contentRoot = GetProjectPath("src", typeof(TStartup).Assembly);

            var builder = WebHost.CreateDefaultBuilder()
                .UseEnvironment(TestsSettings.Environment)
                .UseContentRoot(contentRoot)
                .ConfigureAppConfiguration((context, configurationBuilder) =>
                {
                    configurationBuilder.AddUserSecrets<TStartup>();
                })
                .ConfigureServices(ConfigureServices)
                .UseSerilog()
                .UseStartup<TStartup>();

            Server = new TestServer(builder);

            Client = Server.CreateClient();
            Client.BaseAddress = new Uri(TestsSettings.BaseAddress);

            Services = Server.Host.Services;
        }

        /// <summary>
        /// Called after tests settings are loaded from config file. Allows for per test settings modification 
        /// </summary>
        /// <param name="settings"> Loaded settings </param>
        public virtual void AfterTestsSettingsLoaded(IntegrationTestsSettings settings)
        {
        }

        private void LoadTestConfiguration()
        {
            var testContentRoot = GetProjectPath("tests", GetType().Assembly);
            var testConfiguration = new ConfigurationBuilder()
                .SetBasePath(testContentRoot)
                .AddJsonFile("appsettings.test.json", false)
                .Build();
            TestsSettings = testConfiguration.Get<IntegrationTestsSettings>();

            AfterTestsSettingsLoaded(TestsSettings);
        }

        /// <summary>
        /// Serializes object and returns http message content 
        /// </summary>
        /// <param name="payload"> Object to serialize </param>
        /// <returns> Http message content as json </returns>
        protected StringContent GetPayload(object payload)
        {
            var serialized = JsonConvert.SerializeObject(payload);

            return new StringContent(serialized, Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Gets the full path to the target project path that we wish to test 
        /// </summary>
        /// <param name="solutionRelativePath">
        /// The parent directory of the target project. e.g. src, samples, test, or test/Websites
        /// </param>
        /// <param name="startupAssembly"> The target project's assembly. </param>
        /// <returns> The full path to the target project. </returns>
        protected static string GetProjectPath(string solutionRelativePath, Assembly startupAssembly)
        {
            // Get name of the target project which we want to test
            var projectName = startupAssembly.GetName().Name;

            // Get currently executing test project path
            var applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;

            // Find the folder which contains the solution file. We then use this information to find
            // the target project which we want to test.
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                var solutionFileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, "NaCoDoKinaApi.sln"));
                if (solutionFileInfo.Exists)
                {
                    return Path.GetFullPath(Path.Combine(directoryInfo.FullName, solutionRelativePath, projectName));
                }

                directoryInfo = directoryInfo.Parent;
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"Solution root could not be located using application root {applicationBasePath}.");
        }

        protected virtual void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IFixture, Fixture>();
            serviceCollection.AddSingleton(TestsSettings);
        }
    }
}