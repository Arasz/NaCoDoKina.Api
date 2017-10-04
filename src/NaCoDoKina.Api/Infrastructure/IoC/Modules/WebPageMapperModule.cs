using AngleSharp;
using Autofac;
using System.Reflection;
using Module = Autofac.Module;

namespace NaCoDoKina.Api.Infrastructure.IoC.Modules
{
    public class WebPageMapperModule : Module
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
                .Where(type => type.Name.EndsWith("WebPageMapper"))
                .AsImplementedInterfaces();
        }
    }
}