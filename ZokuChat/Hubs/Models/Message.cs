namespace ZokuChat.Hubs.Models
{
    public class Message
    {
		public string UserName { get; set; }

		public string UserId { get; set; }

		public string Text { get; set; }

		public bool IsDeleted { get; set; }

		public string ModifiedId { get; set; }

		public string ModifiedUserName { get; set; } 
    }
}
