using Autofac;
using Microsoft.Extensions.Configuration;
using NaCoDoKina.Api.Infrastructure.Extensions;
using System.Linq;

namespace NaCoDoKina.Api.Infrastructure.IoC.Modules
{
    /// <summary>
    /// Loads all settings types in assembly by naming convention 
    /// </summary>
    public class SettingsModule : Module
    {
        private readonly IConfiguration _applicationConfiguration;

        public SettingsModule(IConfiguration applicationConfiguration)
        {
            _applicationConfiguration = applicationConfiguration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var settingsTypes = ThisAssembly.DefinedTypes
                .Where(info => info.Namespace != null)
                .Where(info => info.Namespace.Contains("Settings"))
                .Where(info => info.Name.EndsWith("Settings"))
                .ToArray();

            foreach (var typeInfo in settingsTypes)
            {
                builder
                    .RegisterInstance(_applicationConfiguration.GetSettings(typeInfo))
                    .As(typeInfo);
            }
        }
    }
}