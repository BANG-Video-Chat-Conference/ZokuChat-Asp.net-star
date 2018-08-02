using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ZokuChat.Data;
using ZokuChat.Helpers;
using ZokuChat.Models;
using ZokuChat.Services;

namespace ZokuChat.Pages.Account
{
    public class RegisterModel : PageModel
    {
		private readonly IEmailService _emailService;
		private readonly UserManager<ZokuChatUser> _userManager;
		private readonly SignInManager<ZokuChatUser> _signInManager;

		public RegisterModel(
			IEmailService emailService,
			UserManager<ZokuChatUser> userManager,
			SignInManager<ZokuChatUser> signInManager)
		{
			_emailService = emailService;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[BindProperty]
		public Register Register { get; set; }

        public void OnGet()
        {
        }

		public async Task<IActionResult> OnPostAsync()
		{
			if (!Register.Password.Equals(Register.PasswordConfirm))
			{
				ModelState.AddModelError(string.Empty, "Passwords must match.");
			}

			if (ModelState.IsValid)
			{
				var user = new ZokuChatUser { UserName = Register.UserName, Email = Register.Email };
				var result = await _userManager.CreateAsync(user, Register.Password);
				if (result.Succeeded)
				{
					var callbackUrl = Url.Page(
						UrlHelper.GetConfirmEmailUrl(),
						pageHandler: null,
						values: new { },
						protocol: Request.Scheme);

					_emailService.SendEmailConfirmation(user, callbackUrl);

					return LocalRedirect("/Account/ConfirmEmail");
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