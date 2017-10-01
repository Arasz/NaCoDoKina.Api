using Autofac;
using NaCoDoKina.Api.DataProviders.EntityBuilder;

namespace NaCoDoKina.Api.Infrastructure.IoC.Modules
{
    public class DataProviderModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(EntityBuilder<>))
                .AsImplementedInterfaces();
        }
    }
}