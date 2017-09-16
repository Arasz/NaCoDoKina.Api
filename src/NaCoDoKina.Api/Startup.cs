using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NaCoDoKina.Api.Data;
using Swashbuckle.AspNetCore.Swagger;

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

            services.AddDbContext<ApplicationContext>(builder =>
            builder.UseNpgsql(ConnectionString));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "NaCoDoKina.Api",
                    Version = "v1",
                    Contact = new Contact
                    {
                        Email = "araszkiewiczrafal@gmail.com",
                        Name = "Rafał Araszkiewicz",
                        Url = "arasz.me"
                    },
                });
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
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "NaCoDoKina.APi V1");
            });

            app.UseMvc();
        }
    }
}