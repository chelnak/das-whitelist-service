using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using SFA.DAS.WhitelistService.Core.Entities;
using SFA.DAS.WhitelistService.Core.IServices;
using SFA.DAS.WhitelistService.Core.Services;

namespace SFA.DAS.WhitelistService.Functions
{
    public class WhitelistMessageProcessor
    {
        private readonly ConfigurationEntity _configuration;
        private readonly ISQLServerWhitelistService _azureSqlServiceWhitelistService;

        public WhitelistMessageProcessor(IOptions<ConfigurationEntity> configuration, ISQLServerWhitelistService azureSqlServiceWhitelistService)
        {
            _configuration = configuration.Value;
            _azureSqlServiceWhitelistService = azureSqlServiceWhitelistService;
        }

        [FunctionName("WhitelistMessageProcessor")]
        public void Run([QueueTrigger("process", Connection = "StorageConnectionString")]string item, ILogger log)
        {
            var message = JsonConvert.DeserializeObject<QueueMessageEntity>(item);
            log.LogInformation($"Started processing request {message.Id}");

            _azureSqlServiceWhitelistService.Create(message);

            log.LogInformation($"Finished processing request {message.Id}");
        }
    }
}
