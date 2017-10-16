using ApplicationCore.Entities.Movies;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using CacheManager.Core;
using FluentValidation.AspNetCore;
using Infrastructure.Data;
using Infrastructure.Extensions;
using Infrastructure.Identity;
using Infrastructure.IoC;
using Infrastructure.Settings.Jwt;
using Infrastructure.Settings.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using NaCoDoKina.Api.ActionFilters;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Text;

namespace NaCoDoKina.Api
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
            ConfigureMvc(services);

            ConfigureIdentity(services);

            ConfigureApplicationDataAccess(services);

            ConfigureSwaggerServices(services);

            ConfigureAutoMapper(services);

            ConfigureCache(services);

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule(new ApplicationModule(Configuration));
            ApplicationContainer = builder.Build();

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

        /// <summary>
        /// Configure MVC framework 
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureMvc(IServiceCollection services)
        {
            services
                .AddMvc(options =>
                {
                    //Important order
                    options.Filters.Add<IdValidationActionFilter>();
                    options.Filters.Add<InvalidModelActionFilter>();
                })
                .AddFluentValidation(cfg =>
                {
                    cfg.RegisterValidatorsFromAssemblyContaining<Startup>();
                });
        }

        /// <summary>
        /// Adds auto mapper as dependency and imports all possible extension from assembly 
        /// </summary>
        /// <see cref="https://github.com/AutoMapper/AutoMapper.Extensions.Microsoft.DependencyInjection"/>
        /// <param name="services"></param>
        private void ConfigureAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Movie), typeof(DataContracts.Movies.Movie), typeof(Infrastructure.Models.Movies.Movie));
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

            var jwtSettings = Configuration.GetSettings<JwtSettings>();
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtRegisteredClaimNames.UniqueName,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    };
                });
        }

        /// <summary>
        /// Configure logger 
        /// </summary>
        /// <param name="configuration"> App configuration with logger config section </param>
        private void ConfigureLogger(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Destructure.ByTransforming<Type>(type => type.Name)
                .CreateLogger();
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

        /// <summary>
        /// Configure swagger (web api documentation) 
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureSwaggerServices(IServiceCollection services)
        {
            var settings = Configuration.GetSettings<SwaggerSettings>();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(settings.Version, new Info
                {
                    Title = settings.Title,
                    Version = settings.Version,
                    Description = settings.Description,
                    Contact = new Contact
                    {
                        Email = settings.Contact.Email,
                        Name = settings.Contact.Name,
                        Url = settings.Contact.Url
                    },
                });

                var applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;

                options.IncludeXmlComments(Path.Combine(applicationBasePath, "NaCoDoKina.Api.xml"));
            });
        }

        /// <summary>
        /// Creates connection string with password 
        /// </summary>
        /// <param name="name"> Connection string name </param>
        /// <returns></returns>
        private string ConnectionString(string name) =>
            $"{Configuration.GetConnectionString(name)}" +
            $"{Configuration["Database:Password"]};";

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.ShowRequestHeaders();
                options.RoutePrefix = "documentation";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "NaCoDoKina.APi V1");
            });

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}