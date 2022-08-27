using Microsoft.AspNetCore.Mvc;
using Redis.OM;

namespace NewsletterAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsletterController : ControllerBase
    {
        private readonly ILogger<NewsletterController> _logger;
        private readonly IConfiguration _configuration;
        private readonly RedisConnectionProvider _provider;


        public NewsletterController(ILogger<NewsletterController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SendNewsLetter()
        {

            return Accepted();
        }

    }
}