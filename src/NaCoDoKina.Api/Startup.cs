using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using NaCoDoKina.Api.ActionFilters;
using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.Infrastructure.Extensions;
using NaCoDoKina.Api.Infrastructure.Identity;
using NaCoDoKina.Api.Infrastructure.IoC;
using NaCoDoKina.Api.Infrastructure.Settings;
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
        public IContainer ApplicationContainer { get; private set; }

        public Startup(IConfiguration configuration)
        {
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

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule(new ApplicationModule(Configuration));
            ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
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
                    options.Filters.Add<ValidationActionFilter>();
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
            services.AddAutoMapper(typeof(Startup));
        }

        /// <summary>
        /// Configuration of user authentication 
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureIdentity(IServiceCollection services)
        {
            var connectionString = ConnectionString("IdentityConnection");

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
        private static void ConfigureLogger(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        /// <summary>
        /// Configure data access 
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureApplicationDataAccess(IServiceCollection services)
        {
            var connectionString = ConnectionString("DataConnection");

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
            $"{Configuration["DatabasePassword"]};";

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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