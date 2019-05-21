using System;
using System.Threading.Tasks;

namespace SFA.DAS.WhitelistService.Core
{
    public interface IAzureStorageQueueRepository
    {
        Task NewMessage(AzureStorageQueueMessageEntity message);
    }
}
