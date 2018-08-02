using System.ComponentModel.DataAnnotations;

namespace ZokuChat.Models
{
    public class Register
    {
		[EmailAddress]
		[Required]
		public string Email { get; set; }

		[StringLength(20, MinimumLength = 3)]
		[Required(ErrorMessage = "The Username field is required.")]
		public string UserName { get; set; }

		[StringLength(100, MinimumLength = 10)]
		[Required]
		public string Password { get; set; }

		[StringLength(100, MinimumLength = 10)]
		[Required(ErrorMessage = "The Confirm Password field is required.")]
		public string PasswordConfirm { get; set; }
	}
}
