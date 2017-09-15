using Autofac;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using NaCoDoKina.Api.Services;
using System.Net.Http;

namespace NaCoDoKina.Api.IntegrationTests.Modules
{
    public class BasicServiceDependenciesModule : Module
    {
        protected override void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<HttpClient>()
                .AsSelf()
                .SingleInstance();

            containerBuilder.RegisterType<DebugLoggerProvider>()
                .As<ILoggerProvider>();

            containerBuilder.RegisterType<LoggerFactory>()
                .As<ILoggerFactory>();

            containerBuilder.RegisterGeneric(typeof(Logger<>))
                .As(typeof(ILogger<>));

            containerBuilder.Register(context => new MapperConfiguration(cfg =>
                {
                    cfg.AddProfiles(typeof(ITravelService).Assembly);
                }))
                .As<IConfigurationProvider>()
                .AsSelf();

            containerBuilder.RegisterType<Mapper>()
                .AsImplementedInterfaces();
        }
    }
}