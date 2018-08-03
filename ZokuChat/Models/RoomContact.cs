using System;
using System.ComponentModel.DataAnnotations;

namespace ZokuChat.Models
{
    public class RoomContact
    {
		public int Id { get; set; }

		public int RoomId { get; set; }

		public Guid ContactUid { get; set; }

		public Guid CreatedUID { get; set; }

		public DateTime CreatedDateUtc { get; set; }

		public Guid ModifiedUID { get; set; }

		public DateTime ModifiedDateUtc { get; set; }

		public bool IsKicked { get; set; }
    }
}
