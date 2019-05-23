using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SFA.DAS.WhitelistService.Core.IServices;
using SFA.DAS.WhitelistService.Core.IRepositories;
using SFA.DAS.WhitelistService.Core.Entities;

namespace SFA.DAS.WhitelistService.Core.Services
{
    public class AzureSQLServerWhitelistService : ISQLServerWhitelistService
    {
        private readonly ICloudManagementInitializationRepository _azureCloudManagementInititalizationRepository;
        private readonly ConfigurationEntity _configuration;
        public AzureSQLServerWhitelistService(IOptions<ConfigurationEntity> configuration, ICloudManagementInitializationRepository azureCloudManagementInititalizationRepository)
        {
            _azureCloudManagementInititalizationRepository = azureCloudManagementInititalizationRepository;
            _configuration = configuration.Value;
        }

        public void Create(QueueMessageEntity message)
        {
            // Create azure connection
            var azure = _azureCloudManagementInititalizationRepository.Initialize(_configuration.ClientId, _configuration.ClientSecret, _configuration.TenantId);

            // Create or Update firewall rule
            var sqlServer = azure.SqlServers.GetByResourceGroup(message.ResourceGroupName, message.ResourceName);
            sqlServer.FirewallRules.Define(message.Name)
                        .WithIPAddress(message.IPAddress)
                        .Create();
        }
    }
}