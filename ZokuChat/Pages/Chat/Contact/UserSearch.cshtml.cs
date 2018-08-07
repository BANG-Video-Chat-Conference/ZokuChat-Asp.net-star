using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZokuChat.Data;
using ZokuChat.Models;
using ZokuChat.Services;

namespace ZokuChat.Pages.Chat.Contact
{
    public class UserSearchModel : PageModel
    {
		private readonly Context _context;
		private readonly IUserService _userService;
		private readonly IBlockedUserService _blockedUserService;

		public UserSearchModel(Context context, IUserService userService, IBlockedUserService blockedUserService)
		{
			_context = context;
			_userService = userService;
			_blockedUserService = blockedUserService;
		}

		[BindProperty]
		public string SearchText { get; set; }

		public List<User> Users;

		public void OnGet(string searchText)
        {
			// Bind the search text to what was passed in before searching for users
			SearchText = searchText;
			RetrieveUsers();
        }

		public void OnPost()
		{
			RetrieveUsers();
		}

		private void RetrieveUsers()
		{
			if (!String.IsNullOrWhiteSpace(SearchText))
			{
				// Create search
				List<Guid> blockedIds = _blockedUserService.GetUsersWhoBlockedUser(_context.CurrentUser).Select(u => new Guid(u.Id)).ToList();
				blockedIds.AddRange(_blockedUserService.GetUsersBlockedUsers(_context.CurrentUser).Select(u => new Guid(u.BlockedUID)).ToList());

				UserSearch search = new UserSearch
				{
					SearchText = SearchText,
					MaxResults = 10,
					FilteredIds = blockedIds
				};

				// Retrieve and set users
				Users = _userService.GetUsers(search).ToList();
			}
			else
			{
				// Search text is empty or whitespace so set empty list
				Users = new List<User>();
			}
		}
    }
}