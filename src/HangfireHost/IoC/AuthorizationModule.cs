using Autofac;
using Hangfire.Dashboard;

namespace HangfireHost.IoC
{
    public class AuthorizationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(AuthorizationModule).Assembly)
                .Where(type => type.IsAssignableTo<IDashboardAuthorizationFilter>())
                .AsImplementedInterfaces()
                .AsSelf();
        }
    }
}