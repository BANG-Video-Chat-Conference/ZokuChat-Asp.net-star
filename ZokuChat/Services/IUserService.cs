using System;
using System.Collections.Generic;
using System.Linq;
using ZokuChat.Data;
using ZokuChat.Models;

namespace ZokuChat.Services
{
    public interface IUserService
    {
		User GetUserByUID(string UID);

		IQueryable<User> GetUserByUID(string[] UIDs);

		User GetUserByUserName(string userName);

		string GetUserNameByUID(string UID);

		IQueryable<User> GetUsers(UserSearch search);
    }
}
