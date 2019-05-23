using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using SFA.DAS.WhitelistService.Core;

namespace SFA.DAS.WhitelistService.Infrastructure
{
    public class AzureCloudManagementInitializationRepository : ICloudManagementInitializationRepository
    {
        //private readonly ConfigurationEntity _configuration;
        public AzureCloudManagementInitializationRepository() //IOptions<ConfigurationEntity> configuration
        {
            //_configuration = configuration.Value;
        }

        public IAzure Initialize(string clientId, string clientSecret, string tenantId)
        {
            var credentials = SdkContext.AzureCredentialsFactory
                        .FromServicePrincipal(clientId,
                        clientSecret,
                        tenantId,
                        AzureEnvironment.AzureGlobalCloud);

            var azure = Microsoft.Azure.Management.Fluent.Azure
                .Configure()
                .Authenticate(credentials)
                .WithDefaultSubscription();

            return azure;
        }
    }
}
