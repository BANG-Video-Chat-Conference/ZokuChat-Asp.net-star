using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZokuChat.Data;
using ZokuChat.Helpers;
using ZokuChat.Models;
using ZokuChat.Services;

namespace ZokuChat.Pages.Chat.Contact
{
    public class ViewModel : PageModel
    {
		private readonly Context _context;
		private readonly IUserService _userService;
		private readonly IBlockedUserService _blockedUserService;

		public ViewModel(
			Context context,
			IUserService userService,
			IBlockedUserService blockedUserService)
		{
			_context = context;
			_userService = userService;
			_blockedUserService = blockedUserService;
		}

		public User ViewedUser;

        public void OnGet(string userId)
        {
			ViewedUser = _userService.GetUserByUID(userId);

			if (ViewedUser == null || _blockedUserService.IsUserBlocked(_context.CurrentUser, ViewedUser))
			{
				LocalRedirect(UrlHelper.GetAccessDeniedUrl());
			}
        }
    }
}