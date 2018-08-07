using System.Linq;
using ZokuChat.Data;
using ZokuChat.Models;

namespace ZokuChat.Services
{
    public interface IContactService
    {
		Contact GetContact(int contactId);

		IQueryable<Contact> GetUserContacts(User user);

		Contact GetUserContact(User user, User contact);

		void DeleteContact(Contact contact);

		bool IsUserContact(User user, User contact);
    }
}
