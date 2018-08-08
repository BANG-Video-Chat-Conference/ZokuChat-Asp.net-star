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

		public Contact GetContact(int contactId)
		{
			// Validate
			contactId.Should().BeGreaterThan(0);

			// Retrieve the contact
			return _context.Contacts.Where(c => c.Id == contactId).FirstOrDefault();
		}

		public Contact GetUserContact(User user, User contact)
		{
			// Validate
			user.Should().NotBeNull();
			contact.Should().NotBeNull();

			// Retrieve the contact
			return _context.Contacts.Where(c => c.UserUID.Equals(user.Id) && c.ContactUID.Equals(contact.Id)).FirstOrDefault();
		}

		public IQueryable<Contact> GetUserContacts(User user)
		{
			// Validate
			user.Should().NotBeNull();

			// Retrieve the user's contacts
			return _context.Contacts.Where(c => c.UserUID.Equals(user.Id));
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

			return _context.Contacts.Any(c => c.UserUID.Equals(user.Id) && c.ContactUID.Equals(contact.Id));
		}
	}
}
