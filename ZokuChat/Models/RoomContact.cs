using System;
using System.ComponentModel.DataAnnotations;

namespace ZokuChat.Models
{
    public class RoomContact
    {
		public int Id { get; set; }

		public int RoomId { get; set; }

		public string ContactUID { get; set; }

		public string CreatedUID { get; set; }

		public DateTime CreatedDateUtc { get; set; }

		public string ModifiedUID { get; set; }

		public DateTime ModifiedDateUtc { get; set; }

		public bool IsKicked { get; set; }
    }
}
