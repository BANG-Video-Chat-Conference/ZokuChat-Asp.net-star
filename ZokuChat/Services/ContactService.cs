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

		public void DeleteContact(User actionUser, int contactId)
		{
			Contact contact = _context.Contacts.Where(c => c.Id == contactId).FirstOrDefault();

			if (contact != null)
			{
				// Retrieve the other contact in the pair
				string contactUID = contact.ContactUID;
				Contact pairedContact = _context.Contacts.Where(c => new Guid(c.UserUID).Equals(new Guid(contactUID))).FirstOrDefault();

				if (pairedContact != null)
				{
					_context.Contacts.Remove(pairedContact);
				}

				_context.Contacts.Remove(contact);
				_context.SaveChanges();
			}
		}
	}
}
