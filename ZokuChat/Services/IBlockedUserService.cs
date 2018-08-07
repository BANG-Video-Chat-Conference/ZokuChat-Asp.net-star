using System.Collections.Generic;
using System.Linq;
using ZokuChat.Data;
using ZokuChat.Models;

namespace ZokuChat.Services
{
    public interface IBlockedUserService
    {
		void BlockUser(User blocker, User blocked);

		void UnblockUser(User blocker, User blocked);

		bool IsUserBlocked(User user, User blocker);

		bool AreUsersBlocked(User user, User otherUser);

		IQueryable<BlockedUser> GetUsersBlockedUsers(User user);

		IQueryable<User> GetUsersWhoBlockedUser(User user);
    }
}
