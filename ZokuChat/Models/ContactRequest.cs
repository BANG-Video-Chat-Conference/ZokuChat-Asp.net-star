using System;

namespace ZokuChat.Models
{
    public class ContactRequest
    {
		public int Id { get; set; }

		public string FromUID { get; set; }

		public string ToUID { get; set; }

		public string CreatedUID { get; set; }
	
		public DateTime CreatedDateUtc { get; set; }

		public string ModifiedUID { get; set; }

		public DateTime ModifiedDateUtc { get; set; }

		public bool IsConfirmed { get; set; }

		public bool IsCancelled { get; set; }
    }
}
