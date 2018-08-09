using FluentAssertions;
using System;
using System.Linq;
using ZokuChat.Data;
using ZokuChat.Models;

namespace ZokuChat.Services
{
	public class ContactRequestService : IContactRequestService
	{
		private Context _context;

		public ContactRequestService(Context context)
		{
			_context = context;
		}

		public ContactRequest GetContactRequest(int requestId)
		{
			// Validate
			requestId.Should().BeGreaterThan(0);

			// Retrieve
			return _context.ContactRequests.Where(r => r.Id == requestId).FirstOrDefault();
		}

		public void CancelContactRequest(User actionUser, ContactRequest request)
		{
			// Validate
			actionUser.Should().NotBeNull();
			request.Should().NotBeNull();

			// Cancel
			request.IsCancelled = true;
			request.ModifiedUID = actionUser.Id;
			request.ModifiedDateUtc = DateTime.UtcNow;

			// Save
			_context.SaveChanges();
		}

		public void ConfirmContactRequest(User actionUser, ContactRequest request)
		{
			// Validate
			actionUser.Should().NotBeNull();
			request.Should().NotBeNull();

			// Confirm
			request.IsConfirmed = true;
			request.ModifiedUID = actionUser.Id;
			request.ModifiedDateUtc = DateTime.UtcNow;

			// Add contact and paired contact
			Contact contact = new Contact
			{
				UserUID = request.RequestorUID,
				ContactUID = request.RequestedUID
			};

			Contact pairedContact = new Contact
			{
				UserUID = request.RequestedUID,
				ContactUID = request.RequestorUID
			};

			// Add contacts and then set paired ids
			_context.Contacts.AddRange(new Contact[] { contact, pairedContact });
			contact.PairedId = pairedContact.Id;
			pairedContact.PairedId = contact.Id;

			// Now we are ready to save
			_context.SaveChanges();
		}

		public void CreateContactRequest(User requestor, User requested)
		{
			// Validate
			requestor.Should().NotBeNull();
			requested.Should().NotBeNull();

			// Active contact request does not already exist so create one
			DateTime now = DateTime.UtcNow;

			ContactRequest request = new ContactRequest
			{
				RequestorUID = requestor.Id,
				RequestedUID = requested.Id,
				IsCancelled = false,
				IsConfirmed = false,
				CreatedUID = requestor.Id,
				CreatedDateUtc = now,
				ModifiedUID = requestor.Id,
				ModifiedDateUtc = now
			};

			// Save
			_context.ContactRequests.Add(request);
			_context.SaveChanges();
		}

		public IQueryable<ContactRequest> GetContactRequestsFromUser(User user)
		{
			// Validate
			user.Should().NotBeNull();

			return _context.ContactRequests.Where(r => r.RequestorUID.Equals(user.Id));
		}

		public IQueryable<ContactRequest> GetContactRequestsToUser(User user)
		{
			// Validate
			user.Should().NotBeNull();

			return _context.ContactRequests.Where(r => r.RequestedUID.Equals(user.Id));
		}

		public bool HasActiveContactRequest(User requestor, User requested)
		{
			// Validate
			requestor.Should().NotBeNull();
			requested.Should().NotBeNull();

			// Get fromUser's contact requests for toUser
			IQueryable<ContactRequest> requests = GetContactRequestsFromUserToUser(requestor, requested);

			// See if there are any active ones
			return requests.Any(r => r.IsContactRequestActive());
		}

		public IQueryable<ContactRequest> GetContactRequestsFromUserToUser(User requestor, User requested)
		{
			// Validate
			requestor.Should().NotBeNull();
			requested.Should().NotBeNull();

			// Return fromUser's contact requests to toUser
			return _context.ContactRequests.Where(r => r.RequestorUID.Equals(requestor.Id) && r.RequestedUID.Equals(requested.Id));
		}
	}
}
