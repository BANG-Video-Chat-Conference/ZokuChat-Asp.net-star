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

		public void OnGet(string searchText)
        {
			if (!String.IsNullOrWhiteSpace(searchText))
			{
				Users = _userService.GetUsers(new UserSearch { SearchText = searchText, EmailConfirmed = true, MaxResults = 10 });
			}
			else
			{
				Users = new List<User>();
			}
        }

		public void OnPost()
		{
			if (!String.IsNullOrWhiteSpace(SearchText))
			{
				Users = _userService.GetUsers(new UserSearch { SearchText = SearchText, EmailConfirmed = true, MaxResults = 10 });
			}
			else
			{
				Users = new List<User>();
			}
		}
    }
}