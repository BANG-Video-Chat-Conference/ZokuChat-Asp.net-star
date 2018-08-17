namespace ZokuChat.Models
{
    public class MessageSearch
    {
		public string SearchText { get; set; } = string.Empty;

		public int? MaxResults { get; set; } = 300;
	}
}
