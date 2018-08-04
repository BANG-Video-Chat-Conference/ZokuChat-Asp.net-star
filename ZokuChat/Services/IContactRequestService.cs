using System;
using System.Collections.Generic;
using ZokuChat.Data;
using ZokuChat.Models;

namespace ZokuChat.Services
{
    public interface IContactRequestService
    {
		List<ContactRequest> GetContactRequestsToUser(User user);

		List<ContactRequest> GetContactRequestsFromUser(User user);

		void CreateContactRequest(User fromUser, User toUser);

		void CancelContactRequest(User actionUser, int requestId);

		void ConfirmContactRequest(User actionUser, int requestId);

		ContactRequest GetContactRequest(int requestId);
	}
}
