using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace NewsletterProcessor
{
    public class NewsletterQueueProcessor
    {
        [FunctionName("NewsletterQueueProcessor")]
        public void Run([QueueTrigger("NewsletterQueue", Connection = "")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        }
    }
}
