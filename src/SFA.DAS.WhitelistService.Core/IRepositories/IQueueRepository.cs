using System;
using System.Threading.Tasks;

namespace SFA.DAS.WhitelistService.Core
{
    public interface IQueueRepository
    {
        Task NewMessage(QueueMessageEntity message);
    }
}
