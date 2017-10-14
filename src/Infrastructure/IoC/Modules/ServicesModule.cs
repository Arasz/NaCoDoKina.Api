using Autofac;
using Infrastructure.Services;

namespace Infrastructure.IoC.Modules
{
    public class ServicesModule : NamingConventionModule
    {
        protected override string ConventionSuffix => "Service";

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<EuclideanTravelInformationEstimator>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}