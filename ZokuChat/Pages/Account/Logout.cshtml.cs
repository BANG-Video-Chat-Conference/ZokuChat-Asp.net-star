using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZokuChat.Data;
using ZokuChat.Helpers;

namespace ZokuChat.Pages.Account
{
    public class LogoutModel : PageModel
    {
		private readonly SignInManager<ZokuChatUser> _signInManager;

		public LogoutModel(SignInManager<ZokuChatUser> signInManager)
		{
			_signInManager = signInManager;
		}

        public void OnGet()
        {
        }

		public async Task<IActionResult> OnPost(string returnUrl)
		{
			// Set return url to index if necessary
			if (string.IsNullOrWhiteSpace(returnUrl))
			{
				returnUrl = UrlHelper.GetHomeUrl();
			}

			// Sign out and redirect
			await _signInManager.SignOutAsync();
			return LocalRedirect(returnUrl);
		}
    }
}