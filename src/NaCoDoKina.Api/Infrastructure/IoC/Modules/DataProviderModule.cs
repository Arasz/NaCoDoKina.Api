using Autofac;
using NaCoDoKina.Api.DataProviders;
using NaCoDoKina.Api.DataProviders.EntityBuilder;
using NaCoDoKina.Api.DataProviders.Tasks;
using System.Reflection;
using Module = Autofac.Module;

namespace NaCoDoKina.Api.Infrastructure.IoC.Modules
{
    public class DataProviderModule : Module
    {
        protected override Assembly ThisAssembly => GetType().Assembly;

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(EntityBuilder<>))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(type => type.Name.Contains("BuildStep"))
                .AsImplementedInterfaces();

            builder.RegisterType<WebClient>()
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(type => type.IsAssignableTo<IParsableRequestData>())
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(type => type.IsAssignableTo<ITask>())
                .AsSelf();
        }
    }
}