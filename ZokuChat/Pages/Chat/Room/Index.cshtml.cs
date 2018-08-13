using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZokuChat.Models;
using ZokuChat.Services;

namespace ZokuChat.Pages.Chat.Room
{
    public class IndexModel : PageModel
    {
		private readonly Context _context;
		private readonly IRoomService _roomService;

		public IndexModel(Context context, IRoomService roomService)
		{
			_context = context;
			_roomService = roomService;
		}

		public List<Models.Room> Rooms;

        public async Task<IActionResult> OnGet()
        {
			await Task.Run(() => Rooms = _roomService.GetRoomsForUser(_context.CurrentUser).ToList());

			return Page();
        }
    }
}