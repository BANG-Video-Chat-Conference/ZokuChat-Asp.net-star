using System;

namespace ZokuChat.Models
{
    public class ContactRequest
    {
		public int Id { get; set; }

		public Guid FromUID { get; set; }

		public Guid ToUID { get; set; }

		public Guid CreatedUID { get; set; }
	
		public DateTime CreatedDateUtc { get; set; }

		public Guid ModifiedUID { get; set; }

		public DateTime ModifiedDateUtc { get; set; }

		public bool IsConfirmed { get; set; }

		public bool IsCancelled { get; set; }
    }
}
