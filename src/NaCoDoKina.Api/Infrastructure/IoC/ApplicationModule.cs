using Autofac;
using Microsoft.Extensions.Configuration;
using NaCoDoKina.Api.Infrastructure.IoC.Modules;

namespace NaCoDoKina.Api.Infrastructure.IoC
{
    /// <inheritdoc/>
    /// <summary>
    /// Responsible for application scope modules registration 
    /// </summary>
    public class ApplicationModule : Module
    {
        private readonly IConfiguration _applicationConfiguration;

        public ApplicationModule(IConfiguration applicationConfiguration)
        {
            _applicationConfiguration = applicationConfiguration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new SettingsModule(_applicationConfiguration));
            builder.RegisterModule<RepositoriesModule>();
            builder.RegisterModule<ServicesModule>();
            builder.RegisterModule<RequestParsersModule>();
            builder.RegisterModule<MapperModule>();
            builder.RegisterModule<GoogleServiceDependenciesModule>();
        }
    }
}