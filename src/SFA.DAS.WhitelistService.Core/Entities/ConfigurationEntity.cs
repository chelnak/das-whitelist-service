using System;

namespace SFA.DAS.WhitelistService.Core
{
    public class ConfigurationEntity
    {
        public string StorageConnectionString { get; set; }
        public string StorageQueueName { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string TenantId { get; set; }
    }
}
