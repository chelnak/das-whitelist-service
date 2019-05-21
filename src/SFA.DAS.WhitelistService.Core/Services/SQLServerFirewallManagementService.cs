using System;
using System.Threading.Tasks;

namespace SFA.DAS.WhitelistService.Core
{
    public class SQLServerFirewallManagementService : ISQLServerFirewallManagementService
    {
        private readonly IAzureStorageQueueRepository azureStorageQueueRepository;
        public SQLServerFirewallManagementService(IAzureStorageQueueRepository _azureStorageQueueRepository)
        {
            azureStorageQueueRepository =_azureStorageQueueRepository;
        }

        public async Task AddWhitelistEntry(AzureStorageQueueMessageEntity message)
        {
            await azureStorageQueueRepository.NewMessage(message);
        }
    }
}
