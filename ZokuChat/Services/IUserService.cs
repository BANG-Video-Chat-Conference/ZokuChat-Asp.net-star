using System;
using System.Collections.Generic;
using System.Linq;
using ZokuChat.Data;
using ZokuChat.Models;

namespace ZokuChat.Services
{
    public interface IUserService
    {
		User GetUserByUID(Guid UID);

		User GetUserByUserName(string userName);

		IQueryable<User> GetUsers(UserSearch search);
    }
}
