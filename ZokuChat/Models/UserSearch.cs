using System;
using System.Collections.Generic;

namespace ZokuChat.Models
{
	public class UserSearch
	{
		public string SearchText { get; set; } = string.Empty;

		public List<string> FilteredIds { get; set; } = new List<string>();

		public int MaxResults { get; set; } = 20;
    }
}
