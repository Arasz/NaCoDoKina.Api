using ApplicationCore.Entities.Movies;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using CacheManager.Core;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.PostgreSql;
using HangfireHost.Tasks;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IdentityModel.Tokens.Jwt;

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
            services.AddMvc();

            ConfigureHangfire(services);
            ConfigureApplicationDataAccess(services);
            ConfigureIdentity(services);

            ConfigureCache(services);
            ConfigureAutoMapper(services);

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule<TasksModule>();
            builder.RegisterModule(new ApplicationModule(Configuration));
            ApplicationContainer = builder.Build();

            AddAutofacContainerToHangfire();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        /// <summary>
        /// Cache configuration 
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureCache(IServiceCollection services)
        {
            services.AddCacheManagerConfiguration(builder => builder
                .WithMicrosoftLogging(factory => factory.AddSerilog())
                .WithMicrosoftMemoryCacheHandle()
                .WithExpiration(ExpirationMode.Sliding, TimeSpan.FromMinutes(5)));

            services.AddCacheManager();
        }

        private void AddAutofacContainerToHangfire()
        {
            GlobalConfiguration.Configuration.UseAutofacActivator(ApplicationContainer);
        }

        /// <summary>
        /// Configuration of user authentication 
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureIdentity(IServiceCollection services)
        {
            var connectionString = ConnectionString("Identity");

            services.AddDbContext<ApplicationIdentityContext>(builder =>
            {
                builder.UseNpgsql(connectionString);
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.ClaimsIdentity.UserIdClaimType = JwtRegisteredClaimNames.UniqueName;
                })
                .AddEntityFrameworkStores<ApplicationIdentityContext>();
        }

        /// <summary>
        /// Configure data access 
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureApplicationDataAccess(IServiceCollection services)
        {
            var connectionString = ConnectionString("NaCoDoKina");

            services.AddDbContext<ApplicationContext>(builder =>
            {
                builder.UseNpgsql(connectionString);
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHangfireDashboard();

            app.UseHangfireServer();

            app.UseMvc();

            if (Env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                new FireCinemaCityTasks().Schedule();
            }
        }

        /// <summary>
        /// Adds auto mapper as dependency and imports all possible extension from assembly 
        /// </summary>
        /// <see cref="https://github.com/AutoMapper/AutoMapper.Extensions.Microsoft.DependencyInjection"/>
        /// <param name="services"></param>
        private void ConfigureAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Movie), typeof(Infrastructure.Models.Movies.Movie));
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
                if (Env.IsDevelopment())
                    cfg.UseMemoryStorage();
                else
                    cfg.UseStorage(new PostgreSqlStorage(connectionString));
            });
        }
    }
}