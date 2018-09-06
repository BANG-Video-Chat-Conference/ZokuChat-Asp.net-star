using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ZokuChat.Data;
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
		private readonly IUserService _userService;
		private readonly IRoomService _roomService;

		public ChatHub(Context context, IUserService userService, IRoomService roomService)
		{
			_context = context;
			_userService = userService;
			_roomService = roomService;
		}

		public async Task JoinRoom(int roomId)
		{
			if (roomId <= 0)
			{
				await ReturnError("Could not join room", "You must specify a room.");
				return;
			}

			Room room = null;
			await Task.Run(() => room = _roomService.GetRoom(roomId));

			if (room == null || !RoomPermissionHelper.CanViewRoom(_context.CurrentUser, room))
			{
				await ReturnError("Could not join room", "You do not have permission, the room may have been deleted.");
				return;
			}

			await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());

			List<Models.Contact> hubContacts = new List<Models.Contact>();
			await Task.Run(() => { 
				hubContacts = 
					_userService.GetUserByUID(room.Contacts.Select(c => c.ContactUID).ToArray())
					.Select(u => new Models.Contact
					{
						Id = room.Contacts.First(c => c.ContactUID.Equals(u.Id)).Id,
						UserUID = u.Id,
						UserName = u.UserName
					})
					.ToList();
			});

			await Clients.Caller.SendAsync("ReceiveContacts", hubContacts);
		}

		public async Task GetMessages(int roomId)
		{
			if (roomId <= 0)
			{
				await ReturnError("Could not retrieve messages", "You must specify a room.");
				return;
			}

			Room room = null;
			await Task.Run(() => room = _roomService.GetRoom(roomId));

			if (room == null || !RoomPermissionHelper.CanViewRoom(_context.CurrentUser, room))
			{
				await ReturnError("Could not retrieve messages", "You do not have permission, the room may have been deleted.");
				return;
			}

			// Retrieve last 300 messages
			List<Models.Message> hubMessages = null;

			await Task.Run(() => {
				List<Message> messages = _roomService.GetMessages(new MessageSearch()).ToList();
				List<User> messageUsers = _userService.GetUserByUID(messages.Select(m => m.CreatedUID).ToArray()).ToList();

				hubMessages = 
					messages.Select(m => new Models.Message {
								Id = m.Id,
								UserName = messageUsers.First(u => m.CreatedUID.Equals(u.Id)).UserName,
								UserId = m.CreatedUID,
								Text = m.IsDeleted ? string.Empty : m.Text,
								IsDeleted = m.IsDeleted,
								ModifiedId = m.ModifiedUID,
								ModifiedUserName = messageUsers.First(u => u.Id.Equals(m.ModifiedUID)).UserName
							})
							.ToList();
			});

			if (hubMessages == null)
			{
				await ReturnError("Could not retrieve messages", "Something went wrong, pleash refresh the page.");
				return;
			}

			await Clients.Caller.SendAsync("ReceiveMessages", hubMessages);
		}

		public async Task DeleteMessage(int roomId, int messageId)
		{
			// Validation
			if (roomId <= 0 || messageId <= 0)
			{
				await ReturnError("Could not delete message", "You must specify a room and message.");
				return;
			}

			// Retrieve message
			Message message = null;
			await Task.Run(() => message = _roomService.GetMessage(messageId));

			// Permission
			if (message == null || !RoomPermissionHelper.CanDeleteMessage(_context.CurrentUser, message))
			{
				await ReturnError("Could not delete message", "You do not have permission, the room may have been deleted.");
				return;
			}

			// Delete	
			await Task.Run(() => _roomService.DeleteMessage(_context.CurrentUser, message));

			// Notify all in group
			Models.Message hubMessage = new Models.Message {
				Id = message.Id,
				UserName = _userService.GetUserNameByUID(message.CreatedUID),
				UserId = message.CreatedUID,
				Text = string.Empty,
				IsDeleted = true,
				ModifiedId = _context.CurrentUser.Id,
				ModifiedUserName = _context.CurrentUser.UserName
			};
			await Clients.Group(roomId.ToString()).SendAsync("ReceiveDeleteMessage", hubMessage);
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
			int messageId = -1;	
			await Task.Run(() => messageId = _roomService.AddMessage(_context.CurrentUser, room, text));

			// Html encode the text of the message
			text = HtmlEncoder.Default.Encode(text);

			// Notify all in group
			Models.Message hubMessage = new Models.Message
			{
				Id = messageId,
				UserName = _context.CurrentUser.UserName,
				UserId = _context.CurrentUser.Id,
				Text = text,
				IsDeleted = false,
				ModifiedId = _context.CurrentUser.Id,
				ModifiedUserName = _context.CurrentUser.UserName
			};
			await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", hubMessage);
		}

		public async Task StartBroadcast(int roomId, Models.Broadcast broadcast)
		{
			// Validation
			if (roomId <= 0)
			{
				await ReturnError("Could not start broadcast", "You must specify a room.");
				return;
			}

			if (broadcast.StreamId.IsNullOrWhitespace())
			{
				await ReturnError("Could not start broadcast", "StreamId must be specified.");
				return;
			}

			// Permission
			Room room = null;
			await Task.Run(() => room = _roomService.GetRoom(roomId));

			if (room == null || !RoomPermissionHelper.CanAddMessage(_context.CurrentUser, room))
			{
				await ReturnError("Could not start broadcast", "You do not have permission, the room may have been deleted.");
				return;
			}

			// Notify all in group
			Models.Broadcast hubBroadcast = new Models.Broadcast
			{
				UserId = _context.CurrentUser.Id,
				StreamId = broadcast.StreamId
			};
			await Clients.Group(roomId.ToString()).SendAsync("ReceiveBroadcast", hubBroadcast);
		}

		public async Task StopBroadcast(int roomId)
		{
			// Validation
			if (roomId <= 0)
			{
				await ReturnError("Could not stop broadcast", "You must specify a room.");
				return;
			}

			// Permission
			Room room = null;
			await Task.Run(() => room = _roomService.GetRoom(roomId));

			if (room == null || !RoomPermissionHelper.CanAddMessage(_context.CurrentUser, room))
			{
				await ReturnError("Could not stop broadcast", "You do not have permission, the room may have been deleted.");
				return;
			}

			// Notify all in group
			Models.Broadcast hubBroadcast = new Models.Broadcast
			{
				UserId = _context.CurrentUser.Id,
				StreamId = string.Empty
			};
			await Clients.Group(roomId.ToString()).SendAsync("ReceiveDeleteBroadcast", hubBroadcast);
		}

		public async Task SendAnswer(int roomId, string answer)
		{
			// Validation
			if (roomId <= 0)
			{
				await ReturnError("Could not send answer", "You must specify a room.");
				return;
			}

			if (answer.IsNullOrWhitespace())
			{
				await ReturnError("Could not send answer", "You must specify an answer.");
				return;
			}

			// Permission
			Room room = null;
			await Task.Run(() => room = _roomService.GetRoom(roomId));

			if (room == null || !RoomPermissionHelper.CanAddMessage(_context.CurrentUser, room))
			{
				await ReturnError("Could not send answer", "You do not have permission, the room may have been deleted.");
				return;
			}

			// Notify others in group
			await Clients.OthersInGroup(roomId.ToString()).SendAsync("ReceiveAnswer", answer);
		}

		public async Task SendOffer(int roomId, string offer)
		{
			// Validation
			if (roomId <= 0)
			{
				await ReturnError("Could not send offer", "You must specify a room.");
				return;
			}

			if (offer.IsNullOrWhitespace())
			{
				await ReturnError("Could not send offer", "You must specify an offer.");
				return;
			}

			// Permission
			Room room = null;
			await Task.Run(() => room = _roomService.GetRoom(roomId));

			if (room == null || !RoomPermissionHelper.CanAddMessage(_context.CurrentUser, room))
			{
				await ReturnError("Could not send offer", "You do not have permission, the room may have been deleted.");
				return;
			}

			// Notify others in group
			await Clients.OthersInGroup(roomId.ToString()).SendAsync("ReceiveOffer", offer);
		}

		public async Task SendCandidate(int roomId, string candidate)
		{
			// Validation
			if (roomId <= 0)
			{
				await ReturnError("Could not send candidate", "You must specify a room.");
				return;
			}

			if (candidate.IsNullOrWhitespace())
			{
				await ReturnError("Could not send candidate", "You must specify a candidate.");
				return;
			}

			// Permission
			Room room = null;
			await Task.Run(() => room = _roomService.GetRoom(roomId));

			if (room == null || !RoomPermissionHelper.CanAddMessage(_context.CurrentUser, room))
			{
				await ReturnError("Could not send candidate", "You do not have permission, the room may have been deleted.");
				return;
			}

			// Notify others in group
			await Clients.OthersInGroup(roomId.ToString()).SendAsync("ReceiveCandidate", candidate);
		}

		public async Task ReturnError(string errorCaption, string errorMessage)
		{
			await Clients.Caller.SendAsync("ReceiveError", errorCaption, errorMessage);
		}
	}
}
