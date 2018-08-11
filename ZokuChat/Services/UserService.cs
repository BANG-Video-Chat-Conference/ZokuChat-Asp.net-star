using FluentAssertions;
using System.Linq;
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

		public User GetUserByUID(string UID)
		{
			// Validate
			UID.Should().NotBeNullOrWhiteSpace();

			return _context.Users.Find(UID);
		}

		public IQueryable<User> GetUserByUID(string[] UIDs)
		{
			// Validate
			UIDs.Should().NotBeEmpty();

			return _context.Users.Where(u => UIDs.Contains(u.Id));
		}

		public User GetUserByUserName(string userName)
		{
			// Validate
			userName.Should().NotBeNullOrWhiteSpace();

			return _context.Users.Where(u => u.UserName.Equals(userName)).FirstOrDefault();
		}

		public IQueryable<User> GetUsers(UserSearch search)
		{
			// Validate
			search.Should().NotBeNull();
			search.SearchText.Should().NotBeNullOrWhiteSpace();
			search.FilteredIds.Should().NotBeNull();

			return _context.Users
				.Where(u => u.UserName.Contains(search.SearchText) && !search.FilteredIds.Contains(u.Id))
				.Take(search.MaxResults);
		}
	}
}
