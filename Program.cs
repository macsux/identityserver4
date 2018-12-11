using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration.Yaml;
using Pivotal.Extensions.Configuration.ConfigServer;

namespace IdentityServerDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(cfg => 
                    cfg.AddYamlFile("appsettings.yml")
                        .AddYamlFile("config-repo/identityserver.yml"))
                .AddConfigServer()

                .UseStartup<Startup>();
    }


}