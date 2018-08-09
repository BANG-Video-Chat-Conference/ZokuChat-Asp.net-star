namespace ZokuChat.Models
{
    public class RoomSearch
    {
		public string SearchText { get; set; } = string.Empty;

		public bool? IsDeleted { get; set; } = false;

		public int MaxResults { get; set; } = 20;
	}
}
