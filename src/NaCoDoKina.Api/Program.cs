using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NaCoDoKina.Api.Infrastructure.Extensions;
using NaCoDoKina.Api.Infrastructure.Settings;
using Serilog;

namespace NaCoDoKina.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseApplicationInsights()
                .ConfigureLogging(builder =>
                {
                    builder.AddSerilog();
                })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        builder.AddUserSecrets<Startup>();
                        return;
                    }

                    var configuration = builder.Build();

                    var keyVaultSettings = configuration.GetSettings<KeyVaultSettings>();
                    builder.AddAzureKeyVault(keyVaultSettings.VaultUrl, keyVaultSettings.ClientId,
                        keyVaultSettings.ClientSecret);
                })
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();
    }
}