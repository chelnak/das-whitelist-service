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

namespace SFA.DAS.WhiteListService.Functions
{
    public class WhitelistMessageProcessor
    {

        [FunctionName("WhitelistMessageProcessor")]
        public static void Run([QueueTrigger("process", Connection = "StorageConnectionString")]string item, ILogger log, ExecutionContext context)
        {

            var config = new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var credentials = SdkContext.AzureCredentialsFactory
                .FromServicePrincipal(config["clientId"],
                config["clientSecret"],
                config["tenantId"],
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



        public string InitializeAzure()
        {
            var credentials = SdkContext.AzureCredentialsFactory
                .FromServicePrincipal(clientId,
                clientSecret,
                tenantId,
                AzureEnvironment.AzureGlobalCloud);
        }

        public string GetConfiguration()
    }
}
