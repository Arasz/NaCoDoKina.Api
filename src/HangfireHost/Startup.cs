using Autofac;
using Autofac.Extensions.DependencyInjection;
using Hangfire;
using Hangfire.PostgreSql;
using Infrastructure.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;

namespace HangfireHost
{
    public class Startup
    {
        private IHostingEnvironment Env { get; }

        public IContainer ApplicationContainer { get; private set; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Env = env;
            Configuration = configuration;

            ConfigureLogger(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            ConfigureHangfire(services);

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule(new ApplicationModule(Configuration));
            ApplicationContainer = builder.Build();

            AddAutofacContainerToHangfire();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        private void AddAutofacContainerToHangfire()
        {
            GlobalConfiguration.Configuration.UseAutofacActivator(ApplicationContainer);
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }

        /// <summary>
        /// Configure logger 
        /// </summary>
        /// <param name="configuration"> App configuration with logger config section </param>
        private static void ConfigureLogger(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Destructure.ByTransforming<Type>(type => type.Name)
                .CreateLogger();
        }

        /// <summary>
        /// Creates connection string with password 
        /// </summary>
        /// <param name="name"> Connection string name </param>
        /// <returns></returns>
        private string ConnectionString(string name) =>
            $"{Configuration.GetConnectionString(name)}" +
            $"{Configuration["Database:Password"]};";

        /// <summary>
        /// Configuration for hangfire job system 
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureHangfire(IServiceCollection services)
        {
            var connectionString = ConnectionString("Jobs");

            services.AddHangfire(cfg =>
            {
                //cfg.UseMemoryStorage();
                cfg.UseStorage(new PostgreSqlStorage(connectionString));
            });
        }
    }
}