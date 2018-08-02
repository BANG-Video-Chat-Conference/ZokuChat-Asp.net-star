using System.ComponentModel.DataAnnotations;
using ZokuChat.Data;

namespace ZokuChat.Models
{
    public class Room
    {
		public int Id { get; set; }

		[StringLength(200, MinimumLength = 1)]
		[Required]
		public string Name { get; set; }

		public ZokuChatUser Creator { get; set; }

		public ZokuChatUser Users { get; set; }
    }
}
