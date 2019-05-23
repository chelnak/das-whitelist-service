using System;
using System.Threading.Tasks;
using SFA.DAS.WhitelistService.Core.Entities;

namespace SFA.DAS.WhitelistService.Core.IServices
{
    public interface ISQLServerWhitelistService
    {
        void Create(QueueMessageEntity message);
    }
}
