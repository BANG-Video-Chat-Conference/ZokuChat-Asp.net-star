using FluentAssertions;
using System;
using System.Linq;
using ZokuChat.Data;
using ZokuChat.Models;

namespace ZokuChat.Helpers
{
	public class ContactRequestPermissionHelper
	{
		private readonly Context _context;	

		public ContactRequestPermissionHelper(Context context)
		{
			_context = context;
		}

		public bool CanMakeContactRequest(User fromUser, User toUser)
		{
			// Validate
			fromUser.Should().NotBeNull();
			toUser.Should().NotBeNull();

			bool result;

			if (_context.ContactRequests.Any(r => 
				new Guid(r.FromUID).Equals(fromUser.Id) && 
				new Guid(r.ToUID).Equals(toUser.Id) &&
				!r.IsCancelled &&
			    !r.IsConfirmed))
			{
				// An active contact request already exists so return false
				result = false;
			}
			else
			{
				result = true;
			}

			return result;
		}
    }
}
