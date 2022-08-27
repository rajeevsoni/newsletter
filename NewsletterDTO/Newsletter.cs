namespace NewsletterDTO
{
    public class Newsletter
    {
        public IList<string> Emails { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

    }
}