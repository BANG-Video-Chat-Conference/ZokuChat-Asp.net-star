using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZokuChat.Data;
using ZokuChat.Models;

namespace ZokuChat.Services
{
	public class UserService : IUserService
	{
		private readonly Context _context;

		public UserService(Context context)
		{
			_context = context;
		}

		public User GetUserByUID(Guid UID)
		{
			// Validate
			UID.Should().NotBeEmpty();

			return _context.Users.Where(u => new Guid(u.Id).Equals(UID)).FirstOrDefault();
		}

		public User GetUserByUserName(string userName)
		{
			// Validate
			userName.Should().NotBeNullOrWhiteSpace();

			return _context.Users.Where(u => u.UserName.Equals(userName)).FirstOrDefault();
		}

		public List<User> GetUsers(UserSearch search)
		{
			// Validate
			search.Should().NotBeNull();
			search.SearchText.Should().NotBeNullOrWhiteSpace();

			return _context.Users
				.Where(u => u.UserName.Contains(search.SearchText) && u.EmailConfirmed == search.EmailConfirmed)
				.Take(search.MaxResults)
				.ToList();
		}
	}
}
