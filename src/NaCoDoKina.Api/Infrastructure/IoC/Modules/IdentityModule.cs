using Autofac;
using NaCoDoKina.Api.Infrastructure.Services.Identity;

namespace NaCoDoKina.Api.Infrastructure.IoC.Modules
{
    public class IdentityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SignInManagerAdapter>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<UserManagerAdapter>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}