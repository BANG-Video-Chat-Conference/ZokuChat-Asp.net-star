using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using ZokuChat.Helpers;
using ZokuChat.Models;
using ZokuChat.Services;

namespace ZokuChat.Pages.Chat.Room
{
    public class EditModel : PageModel
    {
		private readonly Context _context;
		private readonly IRoomService _roomService;
		private readonly IExceptionService _exceptionService;

		[BindProperty]
		public Models.Room Room { get; set; }

		public EditModel(
			Context context,
			IRoomService roomService,
			IExceptionService exceptionService)
		{
			_context = context;
			_roomService = roomService;
			_exceptionService = exceptionService;
		}

        public IActionResult OnGet(int id)
        {
			if (id > 0)
			{
				Room = _roomService.GetRoom(id);
			}

			if (Room == null || !RoomPermissionHelper.CanEditRoom(_context.CurrentUser, Room))
			{
				return LocalRedirect(UrlHelper.GetAccessDeniedUrl());
			}

			return Page();
        }

		public IActionResult OnPost(int id)
		{
			if (ModelState.IsValid)
			{
				try
				{
					Models.Room retrievedRoom = null;

					if (id > 0)
					{
						retrievedRoom = _roomService.GetRoom(id);
					}

					if (retrievedRoom == null || !RoomPermissionHelper.CanEditRoom(_context.CurrentUser, retrievedRoom))
					{
						return LocalRedirect(UrlHelper.GetAccessDeniedUrl());
					}

					// Set the new property values and save
					retrievedRoom.Name = Room.Name;
					retrievedRoom.Description = Room.Description;

					_context.SaveChanges();

					// If we got here we were successful, so redirect to view page
					return LocalRedirect(UrlHelper.GetViewRoomUrl(retrievedRoom.Id));
				}
				catch (Exception e)
				{
					_exceptionService.ReportException(e);
				}
			}

			return Page();
		}
    }
}