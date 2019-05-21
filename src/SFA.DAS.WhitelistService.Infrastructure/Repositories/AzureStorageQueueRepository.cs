using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using SFA.DAS.WhitelistService.Core;

namespace SFA.DAS.WhitelistService.Core
{
    public class AzureStorageQueueRepository : IAzureStorageQueueRepository
    {
        private readonly ConfigurationEntity _configuration;
        public AzureStorageQueueRepository(IOptions<ConfigurationEntity> configuration)
        {
            _configuration = configuration.Value;
        }

        public async Task NewMessage(AzureStorageQueueMessageEntity message)
        {
            // TODO: Move this out
            var storageAccount = CloudStorageAccount.Parse(_configuration.StorageConnectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference(_configuration.StorageQueueName);
            await queue.CreateIfNotExistsAsync();
            
            var messageString = JsonConvert.SerializeObject(message);
            var queueMessage = new CloudQueueMessage(messageString);
            await queue.AddMessageAsync(queueMessage);
        }
    }
}
