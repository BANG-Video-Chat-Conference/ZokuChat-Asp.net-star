namespace ZokuChat.Models
{
	public class UserSearch
	{
		public string SearchText { get; set; } = string.Empty;

		public int MaxResults { get; set; } = 20;
    }
}
