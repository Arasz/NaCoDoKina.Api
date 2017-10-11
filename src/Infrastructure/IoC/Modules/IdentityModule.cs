using Autofac;
using Infrastructure.Services.Identity;

namespace Infrastructure.IoC.Modules
{
    public class IdentityModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JwtClaimAuthenticatedUserId>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}