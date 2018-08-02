using ZokuChat.Data;

namespace ZokuChat.Services
{
    public interface IEmailService
    {
		void SendEmailConfirmation(ZokuChatUser user, string callbackUrl);
    }
}
