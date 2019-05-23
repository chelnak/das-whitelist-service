using System;
using System.Threading.Tasks;
using SFA.DAS.WhitelistService.Core.IServices;
using SFA.DAS.WhitelistService.Core.IRepositories;
using SFA.DAS.WhitelistService.Core.Entities;

namespace SFA.DAS.WhitelistService.Core.Services
{
    public class FirewallMessageManagementService : IFirewallMessageManagementService
    {
        private readonly IQueueRepository queueRepository;
        public FirewallMessageManagementService(IQueueRepository _queueRepository)
        {
            queueRepository =_queueRepository;
        }

        public async Task AddMessage(QueueMessageEntity message)
        {
            await queueRepository.NewMessage(message);
        }
    }
}
