using FluentAssertions;
using ZokuChat.Data;
using ZokuChat.Services;

namespace ZokuChat.Helpers
{
    public class BlockedUserPermissionHelper
    {
		public static bool CanBlockUser(IBlockedUserService blockedUserService, User actionUser, User otherUser)
		{
			// Validate
			actionUser.Should().NotBeNull();
			otherUser.Should().NotBeNull();

			// Return whether or not the action user is part of the contact pair
			return !blockedUserService.AreUsersBlocked(actionUser, otherUser);
		}

		public static bool CanUnblockUser(IBlockedUserService blockedUserService, User actionUser, User blocked)
		{
			// Validate
			actionUser.Should().NotBeNull();
			blocked.Should().NotBeNull();

			// Return whether or not the action user is part of the contact pair
			return blockedUserService.IsUserBlocked(blocked, actionUser);
		}
	}
}
