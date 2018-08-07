using System.Linq;
using ZokuChat.Data;
using ZokuChat.Models;

namespace ZokuChat.Services
{
    public interface IContactRequestService
    {
		IQueryable<ContactRequest> GetContactRequestsToUser(User user);

		IQueryable<ContactRequest> GetContactRequestsFromUser(User user);

		void CreateContactRequest(User fromUser, User toUser);

		void CancelContactRequest(User actionUser, ContactRequest request);

		void ConfirmContactRequest(User actionUser, ContactRequest request);

		ContactRequest GetContactRequest(int requestId);

		IQueryable<ContactRequest> GetUsersContactRequestsByUser(User fromUser, User toUser);

		bool HasActiveContactRequest(User fromUser, User toUser);
	}
}
