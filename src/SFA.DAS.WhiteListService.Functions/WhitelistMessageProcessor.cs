using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.WhiteListService.Functions
{
    public static class WhitelistMessageProcessor
    {
        [FunctionName("WhitelistMessageProcessor")]
        public static void Run([QueueTrigger("process", Connection = "StorageConnectionString")]string item, ILogger log)
        {
            //TODO: Add fluent logic
            log.LogInformation($"Message processed: {item}");
        }
    }
}
