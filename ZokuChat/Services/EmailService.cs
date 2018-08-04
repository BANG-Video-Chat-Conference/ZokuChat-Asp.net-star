using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using ZokuChat.Data;

namespace ZokuChat.Services
{
    public class EmailService : IEmailService
    {
		private readonly IEmailSender _emailSender;
		private readonly UserManager<User> _userManager;

		public EmailService(IEmailSender emailSender, UserManager<User> userManager)
		{
			_emailSender = emailSender;
			_userManager = userManager;
		}

		public async void SendEmailConfirmation(User user, string callbackUrl)
		{
			string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

			StringBuilder constructedUrl = new StringBuilder(HtmlEncoder.Default.Encode($"{callbackUrl}?userId={user.Id}&code="));
			constructedUrl.Append(UrlEncoder.Default.Encode(code).Replace("/", "%2F"));
			
			await _emailSender.SendEmailAsync(
				user.Email,
				"Confirm your email for ZokuChat",
				$"Please confirm your ZokuChat account by <a href='{constructedUrl.ToString()}'>clicking here</a>.");
		}
    }
}
