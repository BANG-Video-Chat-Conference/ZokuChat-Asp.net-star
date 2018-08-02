using System.ComponentModel.DataAnnotations;

namespace ZokuChat.Models
{
	public class Login
	{
		[EmailAddress]
		[Required]
		public string Email { get; set; }

		[StringLength(100, MinimumLength = 10)]
		[Required]
		public string Password { get; set; }
	}
}
