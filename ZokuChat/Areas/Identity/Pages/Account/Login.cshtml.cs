using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ZokuChat.Helpers;
using ZokuChat.Models;

namespace ZokuChat.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
		private ZokuChatContext _context;

		public LoginModel(ZokuChatContext context)
		{
			_context = context;
		}

		[StringLength(20, MinimumLength = 3)]
		[Required]
		[BindProperty]
		public string Email { get; set; }

		[StringLength(100, MinimumLength = 10)]
		[Required]
		[BindProperty]
		public string Password { get; set; }

		public void OnGet()
        {
        }

		public async Task<IActionResult> OnPostAsync()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}

			// We successfully logged the user in so redirect to Contacts
			return RedirectToPage(UrlHelper.GetContactsListUrl());
		}
    }
}