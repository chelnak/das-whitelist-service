using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.WhitelistService.Functions;
using SFA.DAS.WhitelistService.Infrastructure;
using SFA.DAS.WhitelistService.Core;

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
            builder.Services.AddScoped<ICloudManagementInitializationRepository, AzureCloudManagementInitializationRepository>();
            // builder.Services.AddScoped<ISQLFirewallService, AzureSQLFirewallService>();
        }
    }
}