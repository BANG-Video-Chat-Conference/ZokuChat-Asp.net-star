using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZokuChat.Models;
using ZokuChat.Services;

namespace ZokuChat.Pages.Chat.Request
{
    public class IndexModel : PageModel
    {
		private readonly IContactRequestService _contactRequestService;
		private readonly ZokuChatContext _context;

		public List<ContactRequest> ContactRequests;

		public IndexModel(IContactRequestService contactRequestService, ZokuChatContext context)
		{
			_contactRequestService = contactRequestService;
			_context = context;
		}

        public void OnGet()
        {
			ContactRequests = _contactRequestService.GetContactRequestsToUser(_context.CurrentUser);
        }
    }
}