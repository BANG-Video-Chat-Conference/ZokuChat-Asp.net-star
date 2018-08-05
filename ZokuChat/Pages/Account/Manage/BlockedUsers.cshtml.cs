using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using ZokuChat.Models;

namespace ZokuChat.Pages.Account.Manage
{
    public class BlockedUsersModel : PageModel
    {
		private readonly Context _context;

		public List<BlockedUser> BlockedUsers;

		public BlockedUsersModel(Context context)
		{
			_context = context;
		}

        public void OnGet()
        {
			BlockedUsers = _context.BlockedUsers.Where(b => new Guid(b.BlockerUID).Equals(_context.CurrentUser.Id)).ToList();
        }
    }
}