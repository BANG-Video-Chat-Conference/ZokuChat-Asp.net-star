using System.ComponentModel.DataAnnotations;

namespace ZokuChat.Models
{
	public class Login
	{
		[Required(ErrorMessage = "The Username field is required.")]
		public string UserName { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
