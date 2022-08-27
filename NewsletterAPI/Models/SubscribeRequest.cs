using System.ComponentModel.DataAnnotations;

namespace NewsletterAPI.Models
{
    public class SubscribeRequest
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
