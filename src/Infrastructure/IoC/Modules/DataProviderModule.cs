using AngleSharp;
using Autofac;
using Infrastructure.DataProviders.Bindings;
using Infrastructure.DataProviders.Client;
using Infrastructure.DataProviders.EntityBuilder;
using Infrastructure.DataProviders.EntityBuilder.Context;
using Infrastructure.DataProviders.Requests;
using Infrastructure.DataProviders.Tasks;
using System.Reflection;
using Module = Autofac.Module;

namespace Infrastructure.IoC.Modules
{
    public class DataProviderModule : Module
    {
        protected override Assembly ThisAssembly => GetType().Assembly;

        protected override void Load(ContainerBuilder builder)
        {
            var angleSharpConfiguration = Configuration.Default
                .WithDefaultLoader();

            builder
                .Register(context => BrowsingContext.New(angleSharpConfiguration))
                .AsImplementedInterfaces();

            builder
                .RegisterAssemblyTypes(ThisAssembly)
                .Where(type => type.Name.EndsWith("WebPageBinder"))
                .AsImplementedInterfaces()
                .AsSelf();

            builder
                .RegisterGeneric(typeof(CollectionNodeBinder<>))
                .AsSelf()
                .AsImplementedInterfaces();
            builder
                .RegisterGeneric(typeof(UniversalNodeBinder<>))
                .AsSelf()
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(type => type.IsAssignableTo<IEntityBuilderContext>())
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterGeneric(typeof(EntitiesBuilder<,>))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(type => type.Name.Contains("BuildStep"))
                .AsImplementedInterfaces();

            builder.RegisterType<WebClient>()
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(type => type.IsAssignableTo<IParsableRequestData>())
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(type => type.IsAssignableTo<IRequestParameter>())
                .Where(type => type != typeof(RequestParameter))
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(type => type.IsAssignableTo<ITask>())
                .AsSelf();

            builder
                .RegisterAssemblyTypes(ThisAssembly)
                .Where(type => type.Name.EndsWith("ReviewSearch"))
                .AsImplementedInterfaces()
                .AsSelf();
        }
    }
}