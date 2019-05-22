using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
// using Microsoft.Azure.Management.Sql.Fluent.Models;

using Microsoft.Extensions.Logging;
using SFA.DAS.WhitelistService.Core;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace SFA.DAS.WhitelistService.Functions
{
    public class WhitelistMessageProcessor
    {
        private readonly IConfiguration _configuration;
        private readonly ICloudManagementInitializationRepository _azureCloudManagementInititalizationRepository;

        public WhitelistMessageProcessor(IConfiguration configuration, ICloudManagementInitializationRepository azureCloudManagementInititalizationRepository)
        {
            _configuration = configuration;
            _azureCloudManagementInititalizationRepository = azureCloudManagementInititalizationRepository;
        }

        [FunctionName("WhitelistMessageProcessor")]
        public void Run([QueueTrigger("process", Connection = "StorageConnectionString")]string item, ILogger log, ExecutionContext context)
        {
            var clientId = GetConfiguration("ClientId");
            var clientSecret =  GetConfiguration("ClientSecret");
            var tenantId =  GetConfiguration("TenantId");

            var azure = _azureCloudManagementInititalizationRepository.Initialize(clientId, clientSecret, tenantId);

            var message = JsonConvert.DeserializeObject<QueueMessageEntity>(item);
            log.LogInformation($"Started processing request {message.Id}");

            var sqlServer = azure.SqlServers.GetByResourceGroup(message.ResourceGroupName, message.ResourceName);
            sqlServer.FirewallRules.Define(message.Name)
                        .WithIPAddress(message.IPAddress)
                        .Create();

            log.LogInformation($"Finished processing request {message.Id}");

        }

        private string GetConfiguration(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new Exception($"A configuration key was not specified");
            }
            
            return _configuration.GetValue<string>(key) ?? throw new Exception($"Could not find {key} config");
        }
    }
}
