using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NewsletterDTO;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;

namespace NewsletterProcessor
{
    public class NewsletterQueueProcessor
    {
        [FunctionName("NewsletterQueueProcessor")]
        [return: SendGrid(ApiKey = "SENDGRID_API_KEY")]
        public SendGridMessage Run([QueueTrigger("NewsletterQueue", Connection = "NewsletterQueueURI")] Newsletter newsletter, ILogger log, ExecutionContext context)
        {
            log.LogInformation($"C# Queue trigger function processed at: {DateTime.UtcNow}");
            var config = GetConfiguration(context);
            SendGridMessage message = CreateSendGridEmailMessage(newsletter, config);
            return message;
        }

        private IConfiguration GetConfiguration(ExecutionContext context)
        {
            var config = new ConfigurationBuilder()
            .SetBasePath(context.FunctionAppDirectory)
            .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

            return config;
        }

        private SendGridMessage CreateSendGridEmailMessage(Newsletter newsletter, IConfiguration config)
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(config["FromEmail"], config["FromName"]),
                Subject = newsletter.Subject,
                PlainTextContent = newsletter.Body
            };
            msg.AddTo(config["FromEmail"]);
            List<EmailAddress> emailAddresses = new List<EmailAddress>();

            foreach (var email in newsletter.Emails)
            {
                emailAddresses.Add(new EmailAddress(email));
            }

            msg.AddBccs(emailAddresses);
            return msg;
        }
    }
}
