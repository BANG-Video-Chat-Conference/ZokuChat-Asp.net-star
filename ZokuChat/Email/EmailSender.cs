using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using ZokuChat.Email;

namespace WebPWrecover.Services
{
	public class EmailSender : IEmailSender
	{
		public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
		{
			Options = optionsAccessor.Value;
		}

		public AuthMessageSenderOptions Options { get; }

		public Task SendEmailAsync(string email, string subject, string message)
		{
			return Execute(Options.SendGridAPIKeyValue, subject, message, email);
		}

		public Task Execute(string apiKey, string subject, string message, string email)
		{
			// Construct email
			var client = new SendGridClient(apiKey);
			var msg = new SendGridMessage()
			{
				From = new EmailAddress("ZokuBot@ZokuChat.com", "Zoku Bot"),
				Subject = subject,
				PlainTextContent = message,
				HtmlContent = message
			};
			msg.AddTo(new EmailAddress(email));

			// Disable click tracking
			msg.TrackingSettings = new TrackingSettings
			{
				ClickTracking = new ClickTracking { Enable = false }
			};

			// Send asynch
			return client.SendEmailAsync(msg);
		}
	}
}