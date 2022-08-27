using Microsoft.AspNetCore.Mvc;
using NewsletterAPI.Models;
using NewsletterAPI.RedisDocument;
using Redis.OM;

namespace NewsletterAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubscriptionController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly RedisConnectionProvider _provider;


        public SubscriptionController(IConfiguration configuration, RedisConnectionProvider provider)
        {
            _configuration = configuration;
            _provider = provider;
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe([FromBody]SubscribeRequest subscribeRequest)
        {
            if (string.IsNullOrWhiteSpace(subscribeRequest.Email))
            {
                return BadRequest();
            }

            Subscription subscription = new Subscription();
            subscription.Email = subscribeRequest.Email;
            subscription.IsSubscribed = true;
            subscription.SubscribedOn = DateTime.UtcNow;

            var subscriptionCollection = _provider.RedisCollection<Subscription>();
            await subscriptionCollection.InsertAsync(subscription);
            return Created("/subscription", subscription.Id);
        }

        [HttpDelete]
        public async Task<IActionResult> UnSubscribe([FromBody] UnSubscribeRequest unSubscribeRequest)
        {
            if (string.IsNullOrWhiteSpace(unSubscribeRequest.Email))
            {
                return BadRequest();
            }

            var subscriptionCollection = _provider.RedisCollection<Subscription>();
            var existingSubscription = await subscriptionCollection.FirstOrDefaultAsync(x => x.Email == unSubscribeRequest.Email);
            if(existingSubscription == null)
            {
                return NotFound();
            }

            existingSubscription.IsSubscribed = false;
            _provider.Connection.Set(existingSubscription);
            return Ok("/unsubscribed");
        }

        [HttpPost("/InitializeStorage")]
        public async Task<IActionResult> InitializeStorage()
        {
            var provider = new RedisConnectionProvider(_configuration["REDIS_CONNECTION_STRING"]);
            var isSuccess = await provider.Connection.CreateIndexAsync(typeof(Subscription));
            return Created("Storage", null);
        }
    }
}