using Autofac;
using AutoMapper;

namespace NaCoDoKina.Api.Infrastructure.IoC.Modules
{
    public class MapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles(ThisAssembly);
            });

            builder.RegisterInstance(configuration)
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<Mapper>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}