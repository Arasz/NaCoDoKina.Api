using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.Infrastructure.Identity;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;

namespace NaCoDoKina.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            ConfigureIdentity(services);

            ConfigureApplicationDataAccess(services);

            ConfigureSwaggerServices(services);
        }

        private void ConfigureIdentity(IServiceCollection services)
        {
            services.AddDbContext<IdentityDbContext>(builder =>
            {
                builder.UseNpgsql(ConnectionString);
            });

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>();
        }

        private void ConfigureApplicationDataAccess(IServiceCollection services)
        {
            services.AddDbContext<ApplicationContext>(builder =>
            {
                builder.UseNpgsql(ConnectionString);
            });
        }

        private static void ConfigureSwaggerServices(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "NaCoDoKina.Api",
                    Version = "v1",
                    Description = "Web api for NaCoDoKina project",
                    Contact = new Contact
                    {
                        Email = "araszkiewiczrafal@gmail.com",
                        Name = "Rafał Araszkiewicz",
                        Url = "https://github.com/Arasz"
                    },
                });

                var applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;

                options.IncludeXmlComments(Path.Combine(applicationBasePath, "NaCoDoKina.Api.xml"));
            });
        }

        private string ConnectionString =>
            $"{Configuration.GetConnectionString("DefaultConnection")}" +
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

            app.UseMvc();
        }
    }
}