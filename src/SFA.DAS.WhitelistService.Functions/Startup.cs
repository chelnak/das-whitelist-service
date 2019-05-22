using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.WhitelistService.Functions;
using System;

[assembly: WebJobsStartup(typeof(Startup))]
namespace SFA.DAS.WhitelistService.Functions
{
    public class Startup : IWebJobsStartup
    {
        private readonly IConfiguration _configuration;

        public Startup()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", true, true)
                .AddEnvironmentVariables()
                .Build();
        }

        public void Configure(IWebJobsBuilder builder)
        {
            builder.Services.Configure<dynamic>(_configuration);
            // builder.Services.AddScoped<ISQLFirewallService, AzureSQLFirewallService>();
        }
    }
}