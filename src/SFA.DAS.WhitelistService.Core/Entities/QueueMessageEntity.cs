using System;

namespace SFA.DAS.WhitelistService.Core.Entities
{
    public class QueueMessageEntity
    {
        public string Id { get; set; }
        public SupportedMessageTypeEnum Type { get; set; }
        public string Name { get; set; }
        public string IPAddress { get; set; }
        public string ResourceGroupName { get; set; }
        public string ResourceName { get; set; }


    }
}
