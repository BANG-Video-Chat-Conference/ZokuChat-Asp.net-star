using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using ZokuChat.Models;
using ZokuChat.Services;

namespace ZokuChat.Pages.Account.Manage
{
    public class BlockedUsersModel : PageModel
    {
		private readonly Context _context;
		private readonly IBlockedUserService _blockedUserService;

		public List<BlockedUser> BlockedUsers;

		public BlockedUsersModel(Context context, IBlockedUserService blockedUserService)
		{
			_context = context;
			_blockedUserService = blockedUserService;
		}

        public void OnGet()
        {
			BlockedUsers = _blockedUserService.GetBlockedUsersForUser(_context.CurrentUser).ToList();
        }
    }
}