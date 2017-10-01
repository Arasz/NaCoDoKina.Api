using Autofac;
using NaCoDoKina.Api.DataProviders;
using NaCoDoKina.Api.DataProviders.EntityBuilder;

namespace NaCoDoKina.Api.Infrastructure.IoC.Modules
{
    public class DataProviderModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(EntityBuilder<>))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes()
                .Where(type => type.Name.Contains("BuildStep"))
                .AsImplementedInterfaces();

            builder.RegisterType<WebClient>()
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes()
                .Where(type => type.IsAssignableTo<IParsableRequestData>())
                .AsSelf()
                .AsImplementedInterfaces();
        }
    }
}