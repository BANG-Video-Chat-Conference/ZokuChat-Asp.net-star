using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZokuChat.Data;
using ZokuChat.Helpers;
using ZokuChat.Models;
using ZokuChat.Services;

namespace ZokuChat.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
		private UserManager<User> _userManager;
		private IEmailService _emailService;

		public ConfirmEmailModel(UserManager<User> userManager, IEmailService emailService)
		{
			_userManager = userManager;
			_emailService = emailService;
		}

		public string Message = "Confirm your email to activate your account. If you do not receive an email within the next hour, click the Resend button below.";
		public bool IsResendVisible = true;

		[EmailAddress]
		[Required]
		[BindProperty]
		public string EmailAddress { get; set; }

		public void OnGet(Guid userId, string code)
        {
			if (!userId.Equals(Guid.Empty) && !String.IsNullOrWhiteSpace(code))
			{
				User user = _userManager.Users.FirstOrDefault(u => new Guid(u.Id).Equals(userId));
				
				if (user != null)
				{
					var result = _userManager.ConfirmEmailAsync(user, code).Result;

					if (result.Succeeded)
					{
						Message = "Email confirmed! Go ahead and sign in now :3";
						IsResendVisible = false;
					}
				}
			}
        }

		public async void OnPost()
		{
			if (ModelState.IsValid)
			{
				User user = _userManager.Users.FirstOrDefault(u => u.Email.Equals(EmailAddress));

				if (user != null && !user.EmailConfirmed)
				{
					var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
					var callbackUrl = Url.Page(
						UrlHelper.GetConfirmEmailUrl(),
						pageHandler: null,
						values: new { },
						protocol: Request.Scheme);

					_emailService.SendEmailConfirmation(user, callbackUrl);
				}
				else
				{
					Message = $"We do not have an unconfirmed email address on file matching {EmailAddress}.";
				}
			}
			else
			{
				Message = $"Please provide a valid email address.";
			}	
		}
    }
}