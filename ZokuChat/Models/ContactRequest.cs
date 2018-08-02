using System.ComponentModel.DataAnnotations;

namespace ZokuChat.Models
{
    public class ContactRequest
    {
		[Editable(false)]
		public int Id { get; set; }

		[Required]
		public int FromId { get; set; }

		[Required]
		public int ToId { get; set; }
    }
}
