using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ZokuChat.Data;
using ZokuChat.Helpers;
using ZokuChat.Models;

namespace ZokuChat.Pages.Account
{
    public class RegisterModel : PageModel
    {
		private IEmailSender _emailSender;
		private UserManager<ZokuChatUser> _userManager;
		private SignInManager<ZokuChatUser> _signInManager;

		public RegisterModel(
			IEmailSender sender,
			UserManager<ZokuChatUser> userManager,
			SignInManager<ZokuChatUser> signInManager)
		{
			_emailSender = sender;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[BindProperty]
		public Register Register { get; set; }

        public void OnGet()
        {
        }

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl = returnUrl ?? UrlHelper.GetContactsListUrl();
			if (ModelState.IsValid)
			{
				var user = new ZokuChatUser { UserName = Register.UserName, Email = Register.Email };
				var result = await _userManager.CreateAsync(user, Register.Password);
				if (result.Succeeded)
				{
					var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
					var callbackUrl = Url.Page(
						"/Account/ConfirmEmail",
						pageHandler: null,
						values: new { userId = user.Id, code = code },
						protocol: Request.Scheme);

					await _emailSender.SendEmailAsync(Register.Email, "Confirm your email for RokuChat",
						$"Please confirm your RokuChat account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

					await _signInManager.SignInAsync(user, isPersistent: false);
					return LocalRedirect(returnUrl);
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			// If we got this far, something failed, redisplay form
			return Page();
		}
    }
}