using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using ZokuChat.Helpers;
using ZokuChat.Models;

namespace ZokuChat.Pages.Account
{
    public class LoginModel : PageModel
    {
		private ZokuChatContext _context;

		[BindProperty]
		public Login Login { get; set; }

		public LoginModel(ZokuChatContext context)
		{
			_context = context;
		}

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