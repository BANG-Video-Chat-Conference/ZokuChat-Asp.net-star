using System.ComponentModel.DataAnnotations;

namespace ZokuChat.Models
{
    public class Register
    {
		[EmailAddress]
		[Required]
		public string Email { get; set; }

		[StringLength(20, MinimumLength = 3)]
		[Required]
		public string UserName { get; set; }

		[StringLength(100, MinimumLength = 10)]
		[Required]
		public string Password { get; set; }

		[StringLength(100, MinimumLength = 10)]
		[Required]
		public string PasswordConfirm { get; set; }
	}
}
