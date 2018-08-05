using FluentAssertions;
using ZokuChat.Data;
using ZokuChat.Models;
using ZokuChat.Services;

namespace ZokuChat.Helpers
{
	public class ContactRequestPermissionHelper
	{
		private readonly Context _context;
		private readonly IContactRequestService _contactRequestService;

		public ContactRequestPermissionHelper(Context context, IContactRequestService contactRequestService)
		{
			_context = context;
			_contactRequestService = contactRequestService;
		}

		public bool CanMakeContactRequest(User requestor, User requested)
		{
			// Validate
			requestor.Should().NotBeNull();
			requested.Should().NotBeNull();

			bool result;

			if (_contactRequestService.HasActiveContactRequest(requestor, requested))
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
