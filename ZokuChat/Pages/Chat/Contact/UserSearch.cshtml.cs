using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
		public string SearchMessage;

		public async Task<IActionResult> OnGet(string searchText)
        {
			// Bind the search text to what was passed in before searching for users
			SearchText = searchText;
			await Task.Run(() => Search());

			return Page();
        }

		public async Task<IActionResult> OnPost()
		{
			await Task.Run(() => Search());

			return Page();
		}

		private void Search()
		{
			if (!String.IsNullOrWhiteSpace(SearchText))
			{
				// Create a query for blocked Ids (current user's blocked users and users who blocked current user) 
				List<string> filteredIds =
					_blockedUserService.GetUsersWhoBlockedUser(_context.CurrentUser).Select(u => u.Id)
					.Union(_blockedUserService.GetBlockedUsersForUser(_context.CurrentUser).Select(u => u.BlockedUID)).ToList();

				// Add the curernt user's id to filtered Ids
				filteredIds.Add(_context.CurrentUser.Id);

				// Create the search
				UserSearch search = new UserSearch
				{
					SearchText = SearchText,
					MaxResults = 10,
					FilteredIds = filteredIds
				};

				// Retrieve and set users
				Users = _userService.GetUsers(search).ToList();

				if (!Users.Any())
				{
					SearchMessage = "No users could be found matching the search criteria.";
				}
			}
			else
			{
				// Search text is empty or whitespace so set empty list
				Users = new List<User>();
				SearchMessage = "Search for users by entering a username or part of one and clicking the Search button.";
			}
		}
    }
}