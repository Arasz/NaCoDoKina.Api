using AngleSharp;
using Autofac;
using NaCoDoKina.Api.DataProviders.Bindings;
using NaCoDoKina.Api.DataProviders.Client;
using NaCoDoKina.Api.DataProviders.EntityBuilder;
using NaCoDoKina.Api.DataProviders.Requests;
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
            var angleSharpConfiguration = Configuration.Default
                .WithDefaultLoader(setup => setup.IsResourceLoadingEnabled = true)
                .WithJavaScript()
                .WithCss();

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

            builder.RegisterGeneric(typeof(EntitiesBuilder<>))
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
                .Where(type => type.IsAssignableTo<RequestParameter>())
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