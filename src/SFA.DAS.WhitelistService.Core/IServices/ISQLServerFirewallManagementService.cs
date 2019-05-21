using System;
using System.Threading.Tasks;

namespace SFA.DAS.WhitelistService.Core
{
    public interface ISQLServerFirewallManagementService
    {
        Task AddWhitelistEntry(AzureStorageQueueMessageEntity message);
    }
}
