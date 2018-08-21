namespace ZokuChat.Hubs.Models
{
    public class Contact
    {
		public int Id { get; set; }

		public string UserUID { get; set; }

		public string UserName { get; set; }

		public bool ShowActionMenu { get; set; }
	}
}
