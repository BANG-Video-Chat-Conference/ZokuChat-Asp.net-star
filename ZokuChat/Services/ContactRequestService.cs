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

				// Add contacts for each user
				_context.Contacts.Add(new Contact
				{
					UserUID = request.FromUID,
					ContactUID = request.ToUID
				});

				_context.Contacts.Add(new Contact
				{
					UserUID = request.ToUID,
					ContactUID = request.FromUID
				});

				// Save
				_context.SaveChanges();
			}
		}

		public void CreateContactRequest(User fromUser, User toUser)
		{
			// Validate
			fromUser.Should().NotBeNull();
			toUser.Should().NotBeNull();

			if (!_context.ContactRequests.Any(r =>
				new Guid(r.FromUID).Equals(fromUser.Id) &&
				new Guid(r.ToUID).Equals(toUser.Id) &&
				!r.IsCancelled &&
				!r.IsConfirmed))
			{
				// Active contact request does not already exist so create one
				DateTime now = DateTime.UtcNow;

				ContactRequest request = new ContactRequest
				{
					FromUID = fromUser.Id.ToString(),
					ToUID = toUser.Id.ToString(),
					IsCancelled = false,
					IsConfirmed = false,
					CreatedUID = fromUser.ToString(),
					CreatedDateUtc = now,
					ModifiedUID = fromUser.Id.ToString(),
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

			return _context.ContactRequests.Where(r => new Guid(r.FromUID).Equals(user.Id)).ToList();
		}

		public List<ContactRequest> GetContactRequestsToUser(User user)
		{
			// Validate
			user.Should().NotBeNull();

			return _context.ContactRequests.Where(r => new Guid(r.ToUID).Equals(user.Id)).ToList();
		}
	}
}
