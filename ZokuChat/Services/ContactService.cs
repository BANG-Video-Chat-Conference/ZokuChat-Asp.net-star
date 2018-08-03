using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using ZokuChat.Data;
using ZokuChat.Models;

namespace ZokuChat.Services
{
	public class ContactService : IContactService
	{
		private ZokuChatContext _context;

		public ContactService(ZokuChatContext context)
		{
			_context = context;
		}

		public List<Contact> GetUserContacts(ZokuChatUser user)
		{
			// Validate
			user.Should().NotBeNull();

			// Retrieve the user's contacts
			return _context.Contacts.Where(c => new Guid(c.UserUID).Equals(new Guid(user.Id))).ToList();
		}
	}
}
