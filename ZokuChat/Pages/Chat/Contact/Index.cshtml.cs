using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZokuChat.Models;
using ZokuChat.Services;

namespace ZokuChat.Pages.Chat.Contact
{
    public class IndexModel : PageModel
    {
		private readonly Context _context;
		private readonly IContactService _contactService;

		public List<Models.Contact> Contacts;

		public IndexModel(Context context, IContactService contactService)
		{
			_context = context;
			_contactService = contactService;
		}

		public async Task<IActionResult> OnGet()
		{
			await Task.Run(() => Contacts = _contactService.GetUserContacts(_context.CurrentUser).ToList());
			return Page();
		}
    }
}