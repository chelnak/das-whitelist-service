using System;
using System.Threading.Tasks;
using SFA.DAS.WhitelistService.Core.Entities;

namespace SFA.DAS.WhitelistService.Core.IRepositories
{
    public interface IQueueRepository
    {
        Task NewMessage(QueueMessageEntity message);
    }
}
