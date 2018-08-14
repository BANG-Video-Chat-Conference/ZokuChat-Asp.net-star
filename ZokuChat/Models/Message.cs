using System;
using System.ComponentModel.DataAnnotations;

namespace ZokuChat.Models
{
    public class Message
    {
		public int Id { get; set; }

		public int RoomId { get; set; }

		[StringLength(2000, MinimumLength = 1)]
		[Required]
		public string Text { get; set; }

		[StringLength(450)]
		[Required]
		public string CreatedUID { get; set; }

		[Required]
		public DateTime CreatedDateUtc { get; set; }

		[StringLength(450)]
		[Required]
		public string ModifiedUID { get; set; }

		[Required]
		public DateTime ModifiedDateUtc { get; set; }

		public bool IsDeleted { get; set; }

		[Required]
		public Room Room { get; set; }
	}
}
