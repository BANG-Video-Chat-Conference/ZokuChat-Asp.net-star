using System;
using System.Collections.Generic;
using ZokuChat.Data;
using ZokuChat.Models;

namespace ZokuChat.Services
{
    public interface IUserService
    {
		User GetUserByUID(Guid UID);

		User GetUserByUserName(string userName);

		List<User> GetUsers(UserSearch search);
    }
}
