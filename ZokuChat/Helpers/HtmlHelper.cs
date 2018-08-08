using Microsoft.AspNetCore.Html;
using ZokuChat.Data;
using ZokuChat.Models;
using ZokuChat.Services;

namespace ZokuChat.Helpers
{
    public class HtmlHelper
    {
		public static HtmlString GetContactRequestButton(
			Context context,
			IContactRequestService contactRequestService,
			User user,
			string cssClasses = "")
		{
			if (contactRequestService.HasActiveContactRequest(user, context.CurrentUser))
			{
				return new HtmlString($"<button class='btn btn-success {cssClasses}' onclick=\"window.ZokuChat.AcceptRequestButtonClick($(this), '{user.Id}');\">Accept Request</button>");
			}
			else if (contactRequestService.HasActiveContactRequest(context.CurrentUser, user))
			{
				return new HtmlString($"<button class='btn btn-success {cssClasses}' disabled>Request Sent</button>");
			}
			else
			{
				return new HtmlString($"<button class='btn btn-primary {cssClasses}' onclick=\"window.ZokuChat.SendRequestButtonClick($(this), '{user.Id}');\">Send Request</button>");
			}
		}

		public static HtmlString GetContactRemoveButton(Contact contact, string cssClasses = "")
		{
			return new HtmlString($"<button class='btn btn-danger {cssClasses}' onclick=\"\">Remove Contact</button>");
		}

		public static HtmlString GetBlockUserButton(User user, string cssClasses = "")
		{
			return new HtmlString($"<button class='btn btn-danger {cssClasses}' onclick=\"window.ZokuChat.BlockUserButtonClick($(this), '{user.Id}');\">Block User</button>");
		}

		public static HtmlString GetUnblockUserButton(User user, string cssClasses = "")
		{
			return new HtmlString($"<button class='btn btn-danger {cssClasses}' onclick=\"window.ZokuChat.UnblockUserButtonClick($(this), '{user.Id}');\">Unblock User</button>");
		}
	}
}
