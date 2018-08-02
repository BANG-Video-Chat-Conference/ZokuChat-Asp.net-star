using System.ComponentModel.DataAnnotations;

namespace ZokuChat.Models
{
    public class Message
    {
		public int Id { get; set; }

		public int FromId { get; set; }

		public int RoomId { get; set; }

		[StringLength(2000, MinimumLength = 1)]
		[Required]
		public string Text { get; set; }
    }
}
