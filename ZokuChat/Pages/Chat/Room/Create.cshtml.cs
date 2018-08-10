using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using ZokuChat.Helpers;
using ZokuChat.Models;
using ZokuChat.Services;

namespace ZokuChat.Pages.Chat.Room
{
	public class CreateModel : PageModel
	{
		private readonly Context _context;
		private readonly IRoomService _roomService;

		public CreateModel(Context context, IRoomService roomService)
		{
			_context = context;
			_roomService = roomService;
		}

		[BindProperty]
		public Models.Room Room { get; set; }

        public void OnGet()
        {
        }

		public IActionResult OnPost()
		{
			if (ModelState.IsValid)
			{
				try
				{
					// Try and create the room
					_roomService.CreateRoom(_context.CurrentUser, Room);

					// We are successful if we got here, redirect to list of rooms
					return LocalRedirect(UrlHelper.GetRoomsUrl());
				}
				catch (Exception)
				{
					ModelState.AddModelError(string.Empty, "Could not add room.");
				}
			}
			else
			{
				ModelState.AddModelError(string.Empty, "Invalid form submission.");
			}

			return Page();
		}
    }
}