using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZokuChat.Models
{
    public class Room
    {
		public int Id { get; set; }

		[StringLength(80, MinimumLength = 1)]
		[Required]
		public string Name { get; set; }

		[StringLength(300)]
		public string Description { get; set; }

		[StringLength(450)]
		public string CreatedUID { get; set; }

		[Required]
		public DateTime CreatedDateUtc { get; set; }

		[StringLength(450)]
		public string ModifiedUID { get; set; }

		[Required]
		public DateTime ModifiedDateUtc { get; set; }

		[Required]
		public bool IsDeleted { get; set; }

		public List<RoomContact> Contacts { get; set; }

		public List<Message> Messages { get; set; }
	}
}
