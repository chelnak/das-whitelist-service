using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.WhitelistService.Core.Entities;
using SFA.DAS.WhitelistService.Core.IRepositories;
using SFA.DAS.WhitelistService.Core.IServices;
using SFA.DAS.WhitelistService.Core.Services;
using SFA.DAS.WhitelistService.Infrastructure.Repositories;

[assembly: FunctionsStartup(typeof(SFA.DAS.WhitelistService.Functions.Startup))]
namespace SFA.DAS.WhitelistService.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<ConfigurationEntity>()
                .Configure<IConfiguration>((settings, configuration) => { configuration.Bind(settings); });
            builder.Services.AddSingleton<ISQLServerWhitelistService, AzureSQLServerWhitelistService>();
            builder.Services.AddSingleton<ICloudManagementInitializationRepository, AzureCloudManagementInitializationRepository>();
        }
    }
}