using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZokuChat.Models;
using ZokuChat.Services;

namespace ZokuChat.Pages.Chat.Request
{
    public class IndexModel : PageModel
    {
		private readonly IContactRequestService _contactRequestService;
		private readonly Context _context;

		public List<ContactRequest> ContactRequests;

		public IndexModel(IContactRequestService contactRequestService, Context context)
		{
			_contactRequestService = contactRequestService;
			_context = context;
		}

        public async Task<IActionResult> OnGet()
        {
			await Task.Run(() => ContactRequests = _contactRequestService.GetContactRequestsToUser(_context.CurrentUser).Where(r => r.IsActive()).ToList());
			return Page();
        }
    }
}