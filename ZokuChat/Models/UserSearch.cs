namespace ZokuChat.Models
{
	public class UserSearch
	{
		public string SearchText { get; set; } = string.Empty;

		public bool EmailConfirmed { get; set; } = true;

		public int MaxResults { get; set; } = 20;
    }
}
