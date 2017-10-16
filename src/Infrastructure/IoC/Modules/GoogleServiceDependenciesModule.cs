using Autofac;
using Infrastructure.Services.Google.Services;

namespace Infrastructure.IoC.Modules
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