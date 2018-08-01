using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using WebPWrecover.Services;
using ZokuChat.Areas.Identity.Data;
using ZokuChat.Helpers;
using ZokuChat.Models;

namespace ZokuChat.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
		private ZokuChatContext _context;
		private IEmailSender _emailSender;
		private UserManager<ZokuChatUser> _userManager;
		private SignInManager<ZokuChatUser> _signInManager;

		public RegisterModel(
			ZokuChatContext context,
			IEmailSender sender,
			UserManager<ZokuChatUser> userManager,
			SignInManager<ZokuChatUser> signInManager)
		{
			_context = context;
			_emailSender = sender;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[StringLength(100, MinimumLength = 3)]
		[Required]
		[BindProperty]
		public string Email { get; set; }

		[StringLength(20, MinimumLength = 3)]
		[Required]
		[BindProperty]
		public string UserName { get; set; }

		[StringLength(100, MinimumLength = 10)]
		[Required]
		[BindProperty]
		public string Password { get; set; }

		[StringLength(100, MinimumLength = 10)]
		[Required]
		[BindProperty]
		public string PasswordConfirm { get; set; }

        public void OnGet()
        {
        }

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl = returnUrl ?? UrlHelper.GetContactsListUrl();
			if (ModelState.IsValid)
			{
				var user = new ZokuChatUser { UserName = UserName, Email = Email };
				var result = await _userManager.CreateAsync(user, Password);
				if (result.Succeeded)
				{
					var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
					var callbackUrl = Url.Page(
						"/Account/ConfirmEmail",
						pageHandler: null,
						values: new { userId = user.Id, code = code },
						protocol: Request.Scheme);

					await _emailSender.SendEmailAsync(Email, "Confirm your email",
						$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

					await _signInManager.SignInAsync(user, isPersistent: false);
					return Redirect(returnUrl);
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