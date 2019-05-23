using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.WhitelistService.Core;
using SFA.DAS.WhitelistService.Infrastructure;

[assembly: FunctionsStartup(typeof(SFA.DAS.WhitelistService.Functions.Startup))]
namespace SFA.DAS.WhitelistService.Functions
{
    public class Startup : FunctionsStartup
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

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<ConfigurationEntity>()
                .Configure<IConfiguration>((settings, configuration) => { configuration.Bind(settings); });
            builder.Services.AddSingleton<ICloudManagementInitializationRepository, AzureCloudManagementInitializationRepository>();
        }
    }
}