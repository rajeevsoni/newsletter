using Redis.OM.Modeling;

namespace NewsletterAPI.RedisDocument
{
    [Document]
    public partial class Subscription
    {
        [RedisIdField]
        public string Id { get; set; }
        [Indexed(Sortable = true)]
        public string Email { get; set; }

        [Indexed(Sortable = true)]
        public DateTime SubscribedOn { get; set; }

        [Searchable(Sortable = true)]
        public bool IsSubscribed { get; set; }
    }
}
