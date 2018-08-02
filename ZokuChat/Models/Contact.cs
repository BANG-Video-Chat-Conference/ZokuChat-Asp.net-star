using System.ComponentModel.DataAnnotations;

namespace ZokuChat.Models
{
    public class Contact
    {
		public int Id { get; set; }

		public int UserId { get; set; }

		public int ContactId { get; set; }
    }
}
