using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using SFA.DAS.WhitelistService.Core.IRepositories;
using SFA.DAS.WhitelistService.Core.Entities;

namespace SFA.DAS.WhitelistService.Infrastructure.Repositories
{
    public class AzureStorageQueueRepository : IQueueRepository
    {
        private readonly ConfigurationEntity _configuration;
        public AzureStorageQueueRepository(IOptions<ConfigurationEntity> configuration)
        {
            _configuration = configuration.Value;
        }

        public async Task NewMessage(QueueMessageEntity message)
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
