﻿using System;
using System.ComponentModel.DataAnnotations;

namespace ZokuChat.Models
{
    public class Room
    {
		public int Id { get; set; }

		[StringLength(200, MinimumLength = 1)]
		[Required]
		public string Name { get; set; }

		public Guid CreatorUID { get; set; }

		public DateTime CreatedDateUtc { get; set; }

		public Guid ModifiedUID { get; set; }

		public DateTime ModifiedDateUtc { get; set; }

		public bool IsDeleted { get; set; }
    }
}
