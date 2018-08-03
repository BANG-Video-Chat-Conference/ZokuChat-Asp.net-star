using System;
using System.ComponentModel.DataAnnotations;

namespace ZokuChat.Models
{
    public class ContactRequest
    {
		public int Id { get; set; }

		[StringLength(450)]
		[Required]
		public string FromUID { get; set; }

		[StringLength(450)]
		[Required]
		public string ToUID { get; set; }

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

		public bool IsConfirmed { get; set; }

		public bool IsCancelled { get; set; }
    }
}
