using Autofac;
using NaCoDoKina.Api.Infrastructure.Services.Google.Services;

namespace NaCoDoKina.Api.Infrastructure.IoC.Modules
{
    public class GoogleServiceDependenciesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(GoogleServiceDependencies<>))
                .AsSelf();
        }
    }
}