using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SFA.DAS.WhitelistService.Core;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace SFA.DAS.WhitelistService.Functions
{
    public class WhitelistMessageProcessor
    {
        private readonly ConfigurationEntity _configuration;
        private readonly ICloudManagementInitializationRepository _azureCloudManagementInititalizationRepository;

        public WhitelistMessageProcessor(IOptions<ConfigurationEntity> configuration, ICloudManagementInitializationRepository azureCloudManagementInititalizationRepository)
        {
            _configuration = configuration.Value;
            _azureCloudManagementInititalizationRepository = azureCloudManagementInititalizationRepository;
        }

        [FunctionName("WhitelistMessageProcessor")]
        public void Run([QueueTrigger("process", Connection = "StorageConnectionString")]string item, ILogger log)
        {
            var azure = _azureCloudManagementInititalizationRepository.Initialize(_configuration.ClientId, _configuration.ClientSecret, _configuration.TenantId);

            var message = JsonConvert.DeserializeObject<QueueMessageEntity>(item);
            log.LogInformation($"Started processing request {message.Id}");

            var sqlServer = azure.SqlServers.GetByResourceGroup(message.ResourceGroupName, message.ResourceName);
            sqlServer.FirewallRules.Define(message.Name)
                        .WithIPAddress(message.IPAddress)
                        .Create();

            log.LogInformation($"Finished processing request {message.Id}");

        }
    }
}
