﻿using FluentAssertions;
using System;
using System.Linq;
using ZokuChat.Data;
using ZokuChat.Models;

namespace ZokuChat.Services
{
	public class BlockedUserService : IBlockedUserService
	{
		private readonly Context _context;

		public BlockedUserService(Context context)
		{
			_context = context;
		}

		public void BlockUser(User blocker, User blocked)
		{
			// Validate
			blocker.Should().NotBeNull();
			blocked.Should().NotBeNull();

			if (!IsUserBlocked(blocked, blocker))
			{
				DateTime now = DateTime.UtcNow;

				// Remove any contacts
				_context.Contacts.RemoveRange(_context.Contacts.Where(c => c.UserUID.Equals(blocker.Id) || c.UserUID.Equals(blocked.Id)));

				// Cancel any active contact requests
				_context.ContactRequests.Where(r => r.RequestorUID.Equals(blocker.Id) || r.RequestorUID.Equals(blocked.Id))
					.Where(r => r.IsActive())
					.ToList()
					.ForEach(r => {
						r.IsCancelled = true;
						r.ModifiedUID = blocker.Id;
						r.ModifiedDateUtc = now;
					});

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
