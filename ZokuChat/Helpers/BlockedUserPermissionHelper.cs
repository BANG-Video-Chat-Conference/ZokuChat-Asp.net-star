using FluentAssertions;
using System;
using System.Linq;
using ZokuChat.Data;
using ZokuChat.Models;
using ZokuChat.Services;

namespace ZokuChat.Helpers
{
    public class BlockedUserPermissionHelper
    {
		private readonly Context _context;
		private readonly IBlockedUserService _blockedUserService;

		public BlockedUserPermissionHelper(Context context, IBlockedUserService blockedUserService)
		{
			_context = context;
			_blockedUserService = blockedUserService;
		}

		public bool CanBlockUser(User actionUser, User otherUser)
		{
			// Validate
			actionUser.Should().NotBeNull();
			otherUser.Should().NotBeNull();

			// Return whether or not the action user is part of the contact pair
			return !_blockedUserService.AreUsersBlocked(actionUser, otherUser);
		}

		public bool CanUnblockUser(User actionUser, User blocked)
		{
			// Validate
			actionUser.Should().NotBeNull();
			blocked.Should().NotBeNull();

			// Return whether or not the action user is part of the contact pair
			return _blockedUserService.IsUserBlocked(blocked, actionUser);
		}
	}
}
