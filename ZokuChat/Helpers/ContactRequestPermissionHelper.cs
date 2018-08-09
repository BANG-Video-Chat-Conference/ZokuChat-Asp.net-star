using FluentAssertions;
using ZokuChat.Data;
using ZokuChat.Models;
using ZokuChat.Services;

namespace ZokuChat.Helpers
{
	public class ContactRequestPermissionHelper
	{
		public static bool CanMakeContactRequest(
			IContactService contactService,
			IContactRequestService contactRequestService,
			User requestor,
			User requested)
		{
			// Validate
			contactService.Should().NotBeNull();
			contactRequestService.Should().NotBeNull();
			requestor.Should().NotBeNull();
			requested.Should().NotBeNull();

			bool result;

			if (contactRequestService.HasActiveContactRequest(requestor, requested) || contactService.IsUserContact(requestor, requested))
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

		public static bool CanModifyContactRequest(User user, ContactRequest request)
		{
			// Validate
			user.Should().NotBeNull();
			request.Should().NotBeNull();

			return request.IsContactRequestActive() && request.RequestedUID.Equals(user.Id);
		}
    }
}
