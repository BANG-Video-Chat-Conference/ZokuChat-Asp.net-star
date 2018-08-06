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
		private Context _context;

		public ContactService(Context context)
		{
			_context = context;
		}

		public List<Contact> GetUserContacts(User user)
		{
			// Validate
			user.Should().NotBeNull();

			// Retrieve the user's contacts
			return _context.Contacts.Where(c => new Guid(c.UserUID).Equals(new Guid(user.Id))).ToList();
		}

		public void DeleteContact(Contact contact)
		{
			// Validate
			contact.Should().NotBeNull();

			// Find the paired contacts
			List<Contact> contacts = _context.Contacts.Where(c => c.Id == contact.Id || c.Id == contact.PairedId).ToList();

			// Delete and save
			_context.Contacts.RemoveRange(contacts);
			_context.SaveChanges();
		}

		public bool IsUserContact(User user, User contact)
		{
			// Validate
			user.Should().NotBeNull();
			contact.Should().NotBeNull();

			return _context.Contacts.Any(c => new Guid(c.UserUID).Equals(user.Id) && new Guid(c.ContactUID).Equals(contact.Id));
		}
	}
}
