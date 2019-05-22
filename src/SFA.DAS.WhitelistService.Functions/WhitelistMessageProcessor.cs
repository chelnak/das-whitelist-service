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

        public WhitelistMessageProcessor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [FunctionName("WhitelistMessageProcessor")]
        public void Run([QueueTrigger("process", Connection = "StorageConnectionString")]string item, ILogger log, ExecutionContext context)
        {
            var clientId = _configuration.GetValue<string>("ClientId") ?? throw new Exception($"Could not find ClientId config");
            var clientSecret = _configuration.GetValue<string>("ClientSecret") ?? throw new Exception($"Could not find ClientSecret config");
            var tenantId = _configuration.GetValue<string>("TenantId") ?? throw new Exception($"Could not find TenantId config");

            var credentials = SdkContext.AzureCredentialsFactory
                .FromServicePrincipal(clientId,
                clientSecret,
                tenantId,
                AzureEnvironment.AzureGlobalCloud);

            var azure = Microsoft.Azure.Management.Fluent.Azure
                .Configure()
                .Authenticate(credentials)
                .WithDefaultSubscription();

            var message = JsonConvert.DeserializeObject<QueueMessageEntity>(item);
            log.LogInformation($"Started processing request {message.Id}");

            var sqlServer = azure.SqlServers.GetById(message.ResourceId);

            // needs logic?
            sqlServer.FirewallRules.Define(message.Name)
                        .WithIPAddress(message.IPAddress)
                        .Create();

            log.LogInformation($"Finished processing request {message.Id}");
        }
    }
}
