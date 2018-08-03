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

		public void CancelContactRequest(int requestId)
		{
			// Validate
			requestId.Should().BeGreaterThan(0);

			// Retrieve
			ContactRequest request = GetContactRequest(requestId);

			if (request != null)
			{
				// Cancel
				request.IsCancelled = true;

				// Save
				_context.SaveChanges();
			}
		}

		public void ConfirmContactRequest(int requestId)
		{
			// Validate
			requestId.Should().BeGreaterThan(0);

			// Retrieve
			ContactRequest request = GetContactRequest(requestId);

			if (request != null)
			{
				// Confirm
				request.IsConfirmed = true;

				// Save
				_context.SaveChanges();
			}
		}

		public void CreateContactRequest(Guid fromUID, Guid toUID)
		{
			// Validate
			fromUID.Should().NotBeEmpty();
			toUID.Should().NotBeEmpty();

			// Create request
			DateTime now = DateTime.UtcNow;

			ContactRequest request = new ContactRequest
			{
				FromUID = fromUID.ToString(),
				ToUID = toUID.ToString(),
				IsCancelled = false,
				IsConfirmed = false,
				CreatedUID = fromUID.ToString(),
				CreatedDateUtc = now,
				ModifiedUID = toUID.ToString(),
				ModifiedDateUtc = now
			};

			// Save
			_context.ContactRequests.Add(request);
			_context.SaveChanges();
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
