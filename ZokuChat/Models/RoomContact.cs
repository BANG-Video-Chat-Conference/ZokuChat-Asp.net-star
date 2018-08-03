using System;
using System.ComponentModel.DataAnnotations;

namespace ZokuChat.Models
{
    public class RoomContact
    {
		public int Id { get; set; }

		public int RoomId { get; set; }

		[StringLength(450)]
		[Required]
		public string ContactUID { get; set; }

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

		public bool IsKicked { get; set; }
    }
}
