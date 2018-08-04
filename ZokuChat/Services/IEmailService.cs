using ZokuChat.Data;

namespace ZokuChat.Services
{
    public interface IEmailService
    {
		void SendEmailConfirmation(User user, string callbackUrl);
    }
}
