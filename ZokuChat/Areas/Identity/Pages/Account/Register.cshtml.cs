using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ZokuChat.Helpers;
using ZokuChat.Models;

namespace ZokuChat.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
		private ZokuChatContext _context;

		public RegisterModel(ZokuChatContext context)
		{
			_context = context;
		}

		[StringLength(100, MinimumLength = 3)]
		[Required]
		[BindProperty]
		public string Email { get; set; }

		[StringLength(20, MinimumLength = 3)]
		[Required]
		[BindProperty]
		public string Username { get; set; }

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

		public async Task<IActionResult> OnPostAsync()
		{
			if(!ModelState.IsValid)
			{
				return Page();
			}

			

			// We successfully registered the user so redirect to Contacts
			return RedirectToPage(UrlHelper.GetContactsListUrl());
		}
    }
}