using System;

namespace SFA.DAS.WhitelistService.Core
{
    public class AzureStorageQueueMessageEntity
    {
        public SupportedMessageTypeEnum Type { get; set; }
        public string Name { get; set; }
        public string IPAddress { get; set; }
    }
}
