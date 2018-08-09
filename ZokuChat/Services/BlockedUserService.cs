using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using ZokuChat.Data;
using ZokuChat.Models;

namespace ZokuChat.Services
{
	public class BlockedUserService : IBlockedUserService
	{
		private readonly Context _context;
		private readonly IContactService _contactService;
		private readonly IContactRequestService _contactRequestService;

		public BlockedUserService(
			Context context,
			IContactService contactService,
			IContactRequestService contactRequestService)
		{
			_context = context;
			_contactService = contactService;
			_contactRequestService = contactRequestService;
		}

		public void BlockUser(User blocker, User blocked)
		{
			// Validate
			blocker.Should().NotBeNull();
			blocked.Should().NotBeNull();

			if (!IsUserBlocked(blocked, blocker))
			{
				DateTime now = DateTime.UtcNow;

				if (_contactService.IsUserContact(blocker, blocked))
				{
					// The users are contacts so delete the connection
					Contact contact = _contactService.GetUserContact(blocker, blocked);
					_contactService.DeleteContact(contact);
				}
				else
				{
					// The users are not contacts but there may be an active request, so delete if necessary
					if (_contactRequestService.HasActiveContactRequest(blocker, blocked))
					{
						ContactRequest contactRequest = _contactRequestService.GetContactRequestsFromUserToUser(blocker, blocked).Where(r => r.IsActive()).First();
						_contactRequestService.CancelContactRequest(blocker, contactRequest);		
					}
					else if (_contactRequestService.HasActiveContactRequest(blocked, blocker))
					{
						ContactRequest contactRequest = _contactRequestService.GetContactRequestsFromUserToUser(blocked, blocker).Where(r => r.IsActive()).First();
						_contactRequestService.CancelContactRequest(blocker, contactRequest);
					}
				}

				// Block and save
				_context.BlockedUsers.Add(new BlockedUser
				{
					BlockerUID = blocker.Id,
					BlockedUID = blocked.Id,
					CreatedUID = blocker.Id,
					CreatedDateUtc = now,
					ModifiedUID = blocker.Id,
					ModifiedDateUtc = now
				});
				_context.SaveChanges();
			}
		}

		public void UnblockUser(User blocker, User blocked)
		{
			// Validate
			blocker.Should().NotBeNull();
			blocked.Should().NotBeNull();

			// Unblock user and save
			_context.BlockedUsers.RemoveRange(_context.BlockedUsers.Where(b => b.BlockerUID.Equals(blocker.Id) && b.BlockedUID.Equals(blocked.Id)));
			_context.SaveChanges();
		}

		public IQueryable<BlockedUser> GetBlockedUsersForUser(User user)
		{
			// Validate
			user.Should().NotBeNull();

			// Return the user's blocked users
			return _context.BlockedUsers.Where(b => b.BlockerUID.Equals(user.Id));
		}

		public IQueryable<User> GetUsersWhoBlockedUser(User user)
		{
			// Validate
			user.Should().NotBeNull();

			// Return list of users who blocked the user
			IQueryable<string> blockerUIDs = _context.BlockedUsers.Where(b => b.BlockedUID.Equals(user.Id)).Select(b => b.BlockerUID);
			return _context.Users.Where(u => blockerUIDs.Contains(u.Id));
		}

		public bool IsUserBlocked(User user, User blocker)
		{
			// Validate
			user.Should().NotBeNull();
			blocker.Should().NotBeNull();

			return _context.BlockedUsers.Any(b => b.BlockerUID.Equals(blocker.Id) && b.BlockedUID.Equals(user.Id));
		}

		public bool AreUsersBlocked(User user, User otherUser)
		{
			// Validate
			user.Should().NotBeNull();
			otherUser.Should().NotBeNull();

			return _context.BlockedUsers
				.Where(b => b.BlockerUID.Equals(user.Id) && b.BlockedUID.Equals(otherUser.Id))
				.Where(b => b.BlockerUID.Equals(otherUser.Id) && b.BlockedUID.Equals(user.Id))
				.Any();
		}
	}
}
