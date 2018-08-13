using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;
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

		[BindProperty]
		public string ContactUIDs { get; set; }

		public EditModel(
			Context context,
			IRoomService roomService,
			IExceptionService exceptionService)
		{
			_context = context;
			_roomService = roomService;
			_exceptionService = exceptionService;
		}

        public async Task<IActionResult> OnGet(int id)
        {
			if (id > 0)
			{
				Room = await Task.Run(() => _roomService.GetRoom(id));
			}

			if (Room == null || !RoomPermissionHelper.CanEditRoom(_context.CurrentUser, Room))
			{
				return LocalRedirect(UrlHelper.GetAccessDeniedUrl());
			}

			ContactUIDs = String.Join(",", Room.Contacts.Select(c => c.ContactUID).ToArray());
			return Page();
        }

		public async Task<IActionResult> OnPost(int id)
		{
			if (ModelState.IsValid)
			{
				try
				{
					Models.Room retrievedRoom = null;

					if (id > 0)
					{
						retrievedRoom = await Task.Run(() => _roomService.GetRoom(id));
					}

					if (retrievedRoom == null || !RoomPermissionHelper.CanEditRoom(_context.CurrentUser, retrievedRoom))
					{
						return LocalRedirect(UrlHelper.GetAccessDeniedUrl());
					}

					// Set the new property values and save
					retrievedRoom.Name = Room.Name;
					retrievedRoom.Description = Room.Description;

					await _context.SaveChangesAsync();

					// Now add the room contacts (this will only add valid room contacts)
					_roomService.SetRoomContacts(_context.CurrentUser, retrievedRoom, ContactUIDs != null ? ContactUIDs.Split(",") : new string[] { });

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