using System;
using System.Collections.Generic;
using System.Timers;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Validation;


namespace IdentityServerDemo
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            ((IConfigurationRoot) Configuration).AutoRefresh(TimeSpan.FromSeconds(10));
            services.AddTransient<ICorsPolicyService, ConfigCorsPolicyService>();
            services.AddIdentityServer()
                .AddResourceStore<ConfigResourceStore>()
                .AddClientStore<ConfigClientStore>()
                .AddProfileService<ConfigUserProfileService>()
                .AddResourceOwnerValidator<ConfigUserResourceOwnerPasswordValidator>()
                .AddExtensionGrantValidator<DelegationGrantValidator>()
                .AddExtensionGrantValidator<TokenExchangeGrantValidator>()
                .AddDeveloperSigningCredential();
            
            services.AddMvc();
            services.AddOptions();
            services.Configure<SecuritySettings>(Configuration.GetSection("security"));
            Client c;
//            c.RedirectUris
            var builder = new ContainerBuilder();
            builder.Populate(services);
//            builder.RegisterType<DelegationGrantValidator>().As<IExtensionGrantValidator>();
            var container = builder.Build();
            var security = container.Resolve<IOptionsSnapshot<SecuritySettings>>();
            var serviceProvider = new AutofacServiceProvider(container);
            return serviceProvider;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseIdentityServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();

        }
    }
    public static class ConfigurationRootExtensions
    {
        private static readonly List<Timer> Timers = new List<Timer>();

        public static IConfigurationRoot AutoRefresh(this IConfigurationRoot config, TimeSpan timeSpan)
        {
            var myTimer = new Timer();
            myTimer.Elapsed += (sender, args) => config.Reload();
            myTimer.Interval = 10000;
            myTimer.Enabled = true;
            Timers.Add(myTimer);
            return config;
        }
    }
}