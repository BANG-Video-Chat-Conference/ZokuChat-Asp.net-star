using System;
using System.ComponentModel.DataAnnotations;

namespace ZokuChat.Models
{
	public class BlockedUser
	{
		public int Id { get; set; }

		[StringLength(450)]
		[Required]
		public string BlockerUID { get; set; }

		[StringLength(450)]
		[Required]
		public string BlockedUID { get; set; }

		[StringLength(450)]
		[Required]
		public string CreatedUID { get; set; }

		[Required]
		public DateTime CreatedDateUtc { get; set; }
	}
}