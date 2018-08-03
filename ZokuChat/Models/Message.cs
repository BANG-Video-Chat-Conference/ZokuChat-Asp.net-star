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

		public Guid CreatedUID { get; set; }

		public DateTime CreatedDateUtc { get; set; }

		public Guid ModifiedUID { get; set; }

		public DateTime ModifiedDateUtc { get; set; }

		public bool IsDeleted { get; set; }
    }
}
