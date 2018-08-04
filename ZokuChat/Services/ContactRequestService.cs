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
		private ZokuChatContext _context;

		public ContactRequestService(ZokuChatContext context)
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

		public void CancelContactRequest(ZokuChatUser actionUser, int requestId)
		{
			// Validate
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

		public void ConfirmContactRequest(ZokuChatUser actionUser, int requestId)
		{
			// Validate
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
					ContactUID = request.ToUID,
					IsDeleted = false
				});

				_context.Contacts.Add(new Contact
				{
					UserUID = request.ToUID,
					ContactUID = request.FromUID,
					IsDeleted = false
				});

				// Save
				_context.SaveChanges();
			}
		}

		public void CreateContactRequest(Guid fromUID, Guid toUID)
		{
			// Validate
			fromUID.Should().NotBeEmpty();
			toUID.Should().NotBeEmpty();

			if(!_context.ContactRequests.Any(r => new Guid(r.FromUID).Equals(fromUID) && new Guid(r.ToUID).Equals(toUID)))
			{
				// Contact request does not already exist so create it
				DateTime now = DateTime.UtcNow;

				ContactRequest request = new ContactRequest
				{
					FromUID = fromUID.ToString(),
					ToUID = toUID.ToString(),
					IsCancelled = false,
					IsConfirmed = false,
					CreatedUID = fromUID.ToString(),
					CreatedDateUtc = now,
					ModifiedUID = fromUID.ToString(),
					ModifiedDateUtc = now
				};

				// Save
				_context.ContactRequests.Add(request);
				_context.SaveChanges();
			}
		}

		public List<ContactRequest> GetContactRequestsFromUser(ZokuChatUser user)
		{
			// Validate
			user.Should().NotBeNull();

			return _context.ContactRequests.Where(r => new Guid(r.FromUID).Equals(new Guid(user.Id))).ToList();
		}

		public List<ContactRequest> GetContactRequestsToUser(ZokuChatUser user)
		{
			// Validate
			user.Should().NotBeNull();

			return _context.ContactRequests.Where(r => new Guid(r.ToUID).Equals(new Guid(user.Id))).ToList();
		}
	}
}
