using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using ZokuChat.Data;
using ZokuChat.Models;
using ZokuChat.Services;

namespace ZokuChat.Pages.Chat.Contact
{
    public class IndexModel : PageModel
    {
		private readonly ZokuChatContext _context;
		private readonly UserManager<ZokuChatUser> _userManager;
		private readonly IContactService _contactService;

		private List<Models.Contact> _contacts;

		public IndexModel(ZokuChatContext context, UserManager<ZokuChatUser> userManager, IContactService contactService)
		{
			_context = context;
			_userManager = userManager;
			_contactService = contactService;
		}

		public void OnGet()
		{
			_contacts = _contactService.GetUserContacts(_context.CurrentUser);
		}
    }
}