using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using WebPWrecover.Services;
using ZokuChat.Data;
using ZokuChat.Helpers;

namespace ZokuChat.Services
{
    public class EmailService : IEmailService
    {
		private readonly IEmailSender _emailSender;
		private readonly UserManager<ZokuChatUser> _userManager;

		public EmailService(IEmailSender emailSender, UserManager<ZokuChatUser> userManager)
		{
			_emailSender = emailSender;
			_userManager = userManager;
		}

		public async void SendEmailConfirmation(ZokuChatUser user, string callbackUrl)
		{
			var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

			await _emailSender.SendEmailAsync(
				user.Email,
				"Confirm your email for ZokuChat",
				$"Please confirm your RokuChat account by <a href='{HtmlEncoder.Default.Encode($"{callbackUrl}&userId={user.Id}&code={code}")}'>clicking here</a>.");
		}
    }
}
