using System.ComponentModel.DataAnnotations;

namespace ZokuChat.Models
{
    public class Contact
    {
		public int Id { get; set; }

		[StringLength(450)]
		[Required]
		public string UserUID { get; set; }

		[StringLength(450)]
		[Required]
		public string ContactUID { get; set; }

		public int PairedId { get; set; }
    }
}
