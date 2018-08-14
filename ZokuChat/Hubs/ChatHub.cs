﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using ZokuChat.Extensions;
using ZokuChat.Helpers;
using ZokuChat.Models;
using ZokuChat.Services;

namespace ZokuChat.Hubs
{
	[Authorize]
    public class ChatHub : Hub
    {
		private readonly Context _context;
		private readonly IRoomService _roomService;

		public ChatHub(Context context, IRoomService roomService)
		{
			_context = context;
			_roomService = roomService;
		}

		public async Task SendMessage(int roomId, string text)
		{
			// Validation
			if (roomId <= 0)
			{
				await ReturnError("Could not send message", "You must specify a room.");
				return;
			}

			if (text.IsNullOrWhitespace())
			{
				await ReturnError("Could not send message", "Text must not be null or whitespace.");
				return;
			}

			// Permission
			Room room = null;
			await Task.Run(() => room = _roomService.GetRoom(roomId));

			if (room == null || !RoomPermissionHelper.CanAddMessage(_context.CurrentUser, room))
			{
				await ReturnError("Could not send message", "You do not have permission, the room may have been deleted.");
				return;
			}

			// Trim message if necessary
			if (text.Length > 2000)
			{
				text = text.Substring(0, 2000);
			}

			// Add message to room		
			await Task.Run(() => _roomService.AddMessage(_context.CurrentUser, room, text));

			// Notify all in group
			await Clients.All.SendAsync("ReceiveMessage", _context.CurrentUser.UserName, text);
		}

		public async Task ReturnError(string errorCaption, string errorMessage)
		{
			await Clients.Caller.SendAsync("ReceiveError", errorCaption, errorMessage);
		}
	}
}
