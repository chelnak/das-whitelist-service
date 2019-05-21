using System;
using System.Threading.Tasks;

namespace SFA.DAS.WhitelistService.Core
{
    public class SQLServerFirewallManagementService : ISQLServerFirewallManagementService
    {
        private readonly IQueueRepository queueRepository;
        public SQLServerFirewallManagementService(IQueueRepository _queueRepository)
        {
            queueRepository =_queueRepository;
        }

        public async Task AddWhitelistEntry(QueueMessageEntity message)
        {
            await queueRepository.NewMessage(message);
        }
    }
}
