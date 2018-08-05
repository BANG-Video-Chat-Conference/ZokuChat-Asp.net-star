using FluentAssertions;
using System;
using System.Collections.Generic;
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

		public void CancelContactRequest(User actionUser, int requestId)
		{
			// Validate
			actionUser.Should().NotBeNull();
			requestId.Should().BeGreaterThan(0);

			// Retrieve
			ContactRequest request = GetContactRequest(requestId);

			if (request != null)
			{
				// Cancel
				request.IsCancelled = true;
				request.ModifiedUID = actionUser.Id;
				request.ModifiedDateUtc = DateTime.UtcNow;

				// Save
				_context.SaveChanges();
			}
		}

		public void ConfirmContactRequest(User actionUser, int requestId)
		{
			// Validate
			actionUser.Should().NotBeNull();
			requestId.Should().BeGreaterThan(0);

			// Retrieve
			ContactRequest request = GetContactRequest(requestId);

			if (request != null)
			{
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

				// Set the paired Ids
				contact.PairedId = pairedContact.Id;
				pairedContact.PairedId = contact.Id;

				// Add contacts and save
				_context.Contacts.AddRange(new Contact[] { contact, pairedContact });
				_context.SaveChanges();
			}
		}

		public void CreateContactRequest(User requestor, User requested)
		{
			// Validate
			requestor.Should().NotBeNull();
			requested.Should().NotBeNull();

			if (!HasActiveContactRequest(requestor, requested))
			{
				// Active contact request does not already exist so create one
				DateTime now = DateTime.UtcNow;

				ContactRequest request = new ContactRequest
				{
					RequestorUID = requestor.Id.ToString(),
					RequestedUID = requested.Id.ToString(),
					IsCancelled = false,
					IsConfirmed = false,
					CreatedUID = requestor.ToString(),
					CreatedDateUtc = now,
					ModifiedUID = requestor.Id.ToString(),
					ModifiedDateUtc = now
				};

				// Save
				_context.ContactRequests.Add(request);
				_context.SaveChanges();
			}
		}

		public List<ContactRequest> GetContactRequestsFromUser(User user)
		{
			// Validate
			user.Should().NotBeNull();

			return _context.ContactRequests.Where(r => new Guid(r.RequestorUID).Equals(user.Id)).ToList();
		}

		public List<ContactRequest> GetContactRequestsToUser(User user)
		{
			// Validate
			user.Should().NotBeNull();

			return _context.ContactRequests.Where(r => new Guid(r.RequestedUID).Equals(user.Id)).ToList();
		}

		public bool HasActiveContactRequest(User requestor, User requested)
		{
			// Validate
			requestor.Should().NotBeNull();
			requested.Should().NotBeNull();

			// Get fromUser's contact requests for toUser
			List<ContactRequest> requests = GetUsersContactRequestsByUser(requestor, requested);

			// See if there are any active ones
			return requests.Any(r => r.IsContactRequestActive());
		}

		public List<ContactRequest> GetUsersContactRequestsByUser(User requestor, User requested)
		{
			// Validate
			requestor.Should().NotBeNull();
			requested.Should().NotBeNull();

			// Return fromUser's contact requests to toUser
			return _context.ContactRequests.Where(r => new Guid(r.RequestorUID).Equals(requestor.Id) && new Guid(r.RequestedUID).Equals(requested.Id)).ToList();
		}
	}
}
