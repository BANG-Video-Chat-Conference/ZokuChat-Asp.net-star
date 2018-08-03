using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using ZokuChat.Data;
using ZokuChat.Models;

namespace ZokuChat.Pages.Chat.Contact
{
    public class IndexModel : PageModel
    {
		private readonly ZokuChatContext _context;
		private readonly UserManager<ZokuChatUser> _userManager;

		private List<Models.Contact> _contacts;

		public IndexModel(ZokuChatContext context, UserManager<ZokuChatUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public void OnGet()
		{
			_contacts = _context.Contacts.Where(c => new Guid(c.UserUID).Equals(new Guid(_context.CurrentUser.Id))).ToList();
		}
    }
}