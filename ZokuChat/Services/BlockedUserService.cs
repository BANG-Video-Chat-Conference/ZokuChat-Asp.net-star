﻿using FluentAssertions;
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

				// Block and save
				_context.BlockedUsers.Add(new BlockedUser
				{
					BlockerUID = blocker.Id,
					BlockedUID = blocked.Id,
					CreatedUID = blocker.Id,
					CreatedDateUtc = now
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
			_context.BlockedUsers.RemoveRange(_context.BlockedUsers.Where(b => new Guid(b.BlockerUID).Equals(blocker.Id) && new Guid(b.BlockedUID).Equals(blocked.Id)));
			_context.SaveChanges();
		}

		public List<BlockedUser> GetUsersBlockedUsers(User user)
		{
			// Validate
			user.Should().NotBeNull();

			// Return the user's blocked users
			return _context.BlockedUsers.Where(b => new Guid(b.BlockerUID).Equals(user.Id)).ToList();
		}

		public List<User> GetUsersWhoBlockedUser(User user)
		{
			// Validate
			user.Should().NotBeNull();

			// Return list of users who blocked the user
			List<Guid> blockerUIDs = _context.BlockedUsers.Where(b => new Guid(b.BlockedUID).Equals(user.Id)).Select(b => new Guid(b.BlockerUID)).ToList();
			return _context.Users.Where(u => blockerUIDs.Contains(new Guid(u.Id))).ToList();
		}

		public bool IsUserBlocked(User user, User blocker)
		{
			// Validate
			user.Should().NotBeNull();
			blocker.Should().NotBeNull();

			return _context.BlockedUsers.Any(b => new Guid(b.BlockerUID).Equals(blocker.Id) && new Guid(b.BlockedUID).Equals(user.Id));
		}
	}
}
