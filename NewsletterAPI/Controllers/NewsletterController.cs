using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using NewsletterAPI.Models;
using NewsletterAPI.RedisDocument;
using NewsletterDTO;
using Newtonsoft.Json;
using Redis.OM;

namespace NewsletterAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsletterController : ControllerBase
    {
        private readonly RedisConnectionProvider _provider;
        private readonly QueueClient _queueClient;

        public NewsletterController(ILogger<NewsletterController> logger, IConfiguration configuration, RedisConnectionProvider provider, QueueClient queueClient)
        {
            _provider = provider;
            _queueClient = queueClient;
        }

        [HttpPost]
        public async Task<IActionResult> SendNewsLetter(NewsletterRequest newsletterRequest)
        {
            var subscriptionCollection = _provider.RedisCollection<Subscription>();
            var existingSubscription = await subscriptionCollection.ToListAsync();

            if (existingSubscription != null && existingSubscription.Count > 0)
            {
                Newsletter newsletter = new Newsletter();
                newsletter.Subject = newsletterRequest.Subject;
                newsletter.Body = newsletterRequest.Body;
                newsletter.Emails = existingSubscription.Where(x => x.IsSubscribed).Select(x => x.Email).ToList();
                string serializedNewsLetterObject = JsonConvert.SerializeObject(newsletter);
                await _queueClient.SendMessageAsync(serializedNewsLetterObject);
            }

            return Accepted();
        }

    }
}