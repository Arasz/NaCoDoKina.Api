using Autofac.Builder;
using Autofac.Features.Scanning;

namespace Infrastructure.IoC.Modules
{
    public class RequestParsersModule : NamingConventionModule
    {
        protected override string ConventionSuffix => "RequestParser";

        protected override void ConfigureLifetime(IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> registrationBuilder)
        {
            registrationBuilder
                .SingleInstance();
        }
    }
}