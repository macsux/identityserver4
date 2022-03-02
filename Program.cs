using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration.Yaml;
using Microsoft.Extensions.Hosting;
// using Microsoft.Extensions.Hosting;
using Pivotal.Extensions.Configuration.ConfigServer;

namespace IdentityServerDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
                // .ConfigureAppConfiguration(cfg => 
                //     cfg.AddYamlFile("appsettings.yml")
                //         .AddYamlFile("config-repo/identityserver.yml"))
                // .AddConfigServer()
                //
                // .UseStartup<Startup>();
    }


}