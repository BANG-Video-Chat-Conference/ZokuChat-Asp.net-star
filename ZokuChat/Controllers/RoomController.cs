using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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

		public RoomController(Context context, IRoomService roomService)
		{
			_context = context;
			_roomService = roomService;
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
				catch (Exception)
				{
					// If we got here something went wrong so return accordingly
					return LocalRedirect(UrlHelper.GetErrorUrl());
				}
			}

			// If we got here we were successful so redirect to rooms list
			return LocalRedirect(UrlHelper.GetRoomsUrl());
		}
    }
}