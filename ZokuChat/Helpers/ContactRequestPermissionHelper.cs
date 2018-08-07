using FluentAssertions;
using ZokuChat.Data;
using ZokuChat.Models;
using ZokuChat.Services;

namespace ZokuChat.Helpers
{
	public class ContactRequestPermissionHelper
	{
		private readonly Context _context;
		private readonly IContactService _contactService;
		private readonly IContactRequestService _contactRequestService;

		public ContactRequestPermissionHelper(Context context, IContactService contactService, IContactRequestService contactRequestService)
		{
			_context = context;
			_contactService = contactService;
			_contactRequestService = contactRequestService;
		}

		public bool CanMakeContactRequest(User requestor, User requested)
		{
			// Validate
			requestor.Should().NotBeNull();
			requested.Should().NotBeNull();

			bool result;

			if (_contactRequestService.HasActiveContactRequest(requestor, requested) || _contactService.IsUserContact(requestor, requested))
			{
				// The contact or active request already exists so return false
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
