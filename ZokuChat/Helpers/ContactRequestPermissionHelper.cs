using FluentAssertions;
using System;
using System.Linq;
using ZokuChat.Models;

namespace ZokuChat.Helpers
{
	public class ContactRequestPermissionHelper
	{
		private readonly ZokuChatContext _context;	

		public ContactRequestPermissionHelper(ZokuChatContext context)
		{
			_context = context;
		}

		public bool CanMakeContactRequest(Guid fromUID, Guid toUID)
		{
			// Validate
			fromUID.Should().NotBeEmpty();
			toUID.Should().NotBeEmpty();

			bool result;

			if (_context.ContactRequests.Any(r => 
				new Guid(r.FromUID).Equals(fromUID) && 
				new Guid(r.ToUID).Equals(toUID) &&
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
