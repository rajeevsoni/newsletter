using System.ComponentModel.DataAnnotations;

namespace NewsletterAPI.Models
{
    public class UnSubscribeRequest
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
