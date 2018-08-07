using System;
using System.Collections.Generic;

namespace ZokuChat.Models
{
	public class UserSearch
	{
		public string SearchText { get; set; } = string.Empty;

		public List<Guid> FilteredIds { get; set; } = new List<Guid>();

		public int MaxResults { get; set; } = 20;
    }
}
