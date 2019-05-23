using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using SFA.DAS.WhitelistService.Core.IRepositories;

namespace SFA.DAS.WhitelistService.Infrastructure.Repositories
{
    public class AzureCloudManagementInitializationRepository : ICloudManagementInitializationRepository
    {
        public IAzure Initialize(string clientId, string clientSecret, string tenantId)
        {
            var credentials = SdkContext.AzureCredentialsFactory
                        .FromServicePrincipal(clientId,
                        clientSecret,
                        tenantId,
                        AzureEnvironment.AzureGlobalCloud);

            var azure = Azure
                .Configure()
                .Authenticate(credentials)
                .WithDefaultSubscription();

            return azure;
        }
    }
}
