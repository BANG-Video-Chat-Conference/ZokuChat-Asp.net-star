using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZokuChat.Helpers;
using ZokuChat.Models;
using ZokuChat.Services;

namespace ZokuChat.Pages.Chat.Room
{
    public class ChatModel : PageModel
    {
		private readonly Context _context;
		private readonly IRoomService _roomService;

		public Models.Room Room;

		public ChatModel(Context context, IRoomService roomService)
		{
			_context = context;
			_roomService = roomService;
		}

        public async Task<IActionResult> OnGet(int roomId)
        {
			if (roomId > 0)
			{
				await Task.Run(() => Room = _roomService.GetRoom(roomId, includeMessages: true));
			}

			if (Room == null || !RoomPermissionHelper.CanViewRoom(_context.CurrentUser, Room))
			{
				return LocalRedirect(UrlHelper.GetAccessDeniedUrl());
			}

			return Page();
        }

		public string GetUserNameColorClass(Message message)
		{
			if (Room.CreatedUID.Equals(message.CreatedUID)) 
			{
				return "text-creator";
			}
			else
			{
				return "text-contact";
			}
		}
    }
}