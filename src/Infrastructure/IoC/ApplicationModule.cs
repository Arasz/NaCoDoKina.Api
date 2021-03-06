﻿using Autofac;
using Infrastructure.IoC.Modules;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace Infrastructure.IoC
{
    /// <inheritdoc/>
    /// <summary>
    /// Responsible for application scope modules registration 
    /// </summary>
    public class ApplicationModule : Module
    {
        private readonly IConfiguration _applicationConfiguration;

        public ApplicationModule(IConfiguration applicationConfiguration)
        {
            _applicationConfiguration = applicationConfiguration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<HttpClient>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterModule<IdentityModule>();
            builder.RegisterModule(new SettingsModule(_applicationConfiguration));
            builder.RegisterModule<RepositoriesModule>();
            builder.RegisterModule<ServicesModule>();
            builder.RegisterModule<RequestParsersModule>();
            builder.RegisterModule<GoogleServiceDependenciesModule>();
            builder.RegisterModule<DataProviderModule>();
        }
    }
}