using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SFA.DAS.WhitelistService.Web.Models;
using SFA.DAS.WhitelistService.Core.Services;
using SFA.DAS.WhitelistService.Core.IServices;
using SFA.DAS.WhitelistService.Core.IRepositories;
using SFA.DAS.WhitelistService.Core.Entities;
using SFA.DAS.WhitelistService.Infrastructure.Repositories;
using SFA.DAS.ToolService.Authentication.ServiceCollectionExtensions;
using SFA.DAS.ToolService.Authentication.Entities;
using Microsoft.AspNetCore.HttpOverrides;

namespace SFA.DAS.WhitelistService.Web
{
    public class Startup
    {
        private readonly IHostingEnvironment _env;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var authenticationOptions = Configuration.GetSection("Authentication");

            services.Configure<AuthenticationConfigurationEntity>(authenticationOptions);

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddHealthChecks();
            services.AddAuthenticationProviders(authenticationOptions.Get<AuthenticationConfigurationEntity>());
            services.Configure<ConfigurationEntity>(Configuration);
            services.AddSingleton<IFirewallMessageManagementService, FirewallMessageManagementService>();
            services.AddSingleton<ISQLServerWhitelistService, AzureSQLServerWhitelistService>();
            services.AddSingleton<IQueueRepository, AzureStorageQueueRepository>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAntiforgery(options =>
            {
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            app.UseForwardedHeaders();

            app.Use(async (context, next) =>
            {
                if (context.Request.Headers.ContainsKey("X-Original-Host"))
                {
                    var originalHost = context.Request.Headers["X-Original-Host"];
                    logger.LogInformation($"Retrieving X-Original-Host value {originalHost}");
                    context.Request.Headers.Add("Host", originalHost);
                }
                await next.Invoke();
            });

            if (env.IsDevelopment())
            {
                logger.LogInformation($"App is running in development mode: {env.EnvironmentName}");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                logger.LogInformation($"App is running in production mode: {env.EnvironmentName}");
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-Xss-Protection", "1");
                await next();
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UsePathBase("/Whitelist");
            app.UseHealthChecks("/health");

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}