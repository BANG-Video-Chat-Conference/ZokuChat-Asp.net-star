using System;
using System.ComponentModel.DataAnnotations;

namespace ZokuChat.Models
{
    public class Room
    {
		public int Id { get; set; }

		[StringLength(200, MinimumLength = 1)]
		[Required]
		public string Name { get; set; }

		[StringLength(450)]
		[Required]
		public string CreatorUID { get; set; }

		[Required]
		public DateTime CreatedDateUtc { get; set; }

		[StringLength(450)]
		[Required]
		public string ModifiedUID { get; set; }

		[Required]
		public DateTime ModifiedDateUtc { get; set; }

		public bool IsDeleted { get; set; }
    }
}
