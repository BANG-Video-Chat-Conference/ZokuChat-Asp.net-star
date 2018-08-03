using System;

namespace ZokuChat.Models
{
    public class Contact
    {
		public int Id { get; set; }

		public string UserUID { get; set; }

		public string ContactUID { get; set; }

		public bool IsDeleted { get; set; }
    }
}
