using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using System;
using System.Reflection;
using Module = Autofac.Module;

namespace Infrastructure.IoC.Modules
{
    /// <inheritdoc/>
    /// <summary>
    /// Base module for registering types based on naming convention 
    /// </summary>
    public abstract class NamingConventionModule : Module
    {
        protected override Assembly ThisAssembly => GetType().Assembly;

        protected override void Load(ContainerBuilder builder)
        {
            var registrationBuilder = builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(FilterByNamingConvention)
                .Where(type => type.IsClass)
                .AsImplementedInterfaces();

            ConfigureLifetime(registrationBuilder);
        }

        protected virtual void ConfigureLifetime(IRegistrationBuilder<object, ScanningActivatorData, DynamicRegistrationStyle> registrationBuilder)
        {
            registrationBuilder.InstancePerLifetimeScope();
        }

        protected abstract string ConventionSuffix { get; }

        protected bool FilterByNamingConvention(Type type)
        {
            return type.Name.EndsWith(ConventionSuffix);
        }
    }
}