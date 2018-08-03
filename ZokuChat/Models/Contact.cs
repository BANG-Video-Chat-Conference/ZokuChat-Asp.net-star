using System;

namespace ZokuChat.Models
{
    public class Contact
    {
		public int Id { get; set; }

		public Guid UserUID { get; set; }

		public Guid ContactUID { get; set; }

		public bool IsDeleted { get; set; }
    }
}
