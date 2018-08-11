using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using ZokuChat.Controllers.Responses;
using ZokuChat.Extensions;
using ZokuChat.Helpers;
using ZokuChat.Models;
using ZokuChat.Services;

namespace ZokuChat.Controllers
{
	[Route("Rooms")]
	[Authorize]
    public class RoomController : Controller
    {
		private readonly Context _context;
		private readonly IRoomService _roomService;
		private readonly IContactService _contactService;
		private readonly IExceptionService _exceptionService;

		public RoomController(
			Context context,
			IRoomService roomService,
			IContactService contactService,
			IExceptionService exceptionService)
		{
			_context = context;
			_roomService = roomService;
			_contactService = contactService;
			_exceptionService = exceptionService;
		}

		[Route("AddContacts")]
		public JsonResult AddRoomContacts(int roomId, string[] UIDs)
		{
			GenericResponse result = new GenericResponse() { IsSuccessful = false };

			try
			{
				if (roomId < 1 || UIDs.IsNullOrEmpty())
				{
					result.ErrorMessage = "Bad request.";
					return new JsonResult(result);
				}

				Room room = _roomService.GetRoom(roomId);

				if (room == null || !RoomPermissionHelper.CanAddRoomContact(_context.CurrentUser, room))
				{
					result.ErrorMessage = "You do not have permission to add a contact to this room.";
					return new JsonResult(result);
				}

				// Now add the contacts to the room (UIDs not found in the actionUser's contacts will be filtered out)
				_roomService.AddRoomContacts(_context.CurrentUser, room, UIDs);

				// If we got this far we are successful
				result.IsSuccessful = true;
			}
			catch (Exception e)
			{
				result.ErrorMessage = "An exception occurred.";
				_exceptionService.ReportException(e);
			}

			return new JsonResult(result);
		}

		[Route("Delete")]
        public IActionResult DeleteRoom(int roomId)
        {
			Room room = null;

			if (roomId > 0)
			{
				room = _roomService.GetRoom(roomId);
			}

			if (room == null || !RoomPermissionHelper.CanDeleteRoom(_context.CurrentUser, room))
			{
				return LocalRedirect(UrlHelper.GetAccessDeniedUrl());
			}
			else
			{
				try
				{
					// Delete the room
					_roomService.DeleteRoom(room);
				}
				catch (Exception e)
				{
					_exceptionService.ReportException(e);
					return LocalRedirect(UrlHelper.GetErrorUrl());
				}
			}

			// If we got here we were successful so redirect to rooms list
			return LocalRedirect(UrlHelper.GetRoomsUrl());
		}
    }
}