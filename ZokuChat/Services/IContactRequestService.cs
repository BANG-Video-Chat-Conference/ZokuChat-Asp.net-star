using System;
using System.Collections.Generic;
using ZokuChat.Data;
using ZokuChat.Models;

namespace ZokuChat.Services
{
    public interface IContactRequestService
    {
		List<ContactRequest> GetContactRequestsToUser(ZokuChatUser user);

		List<ContactRequest> GetContactRequestsFromUser(ZokuChatUser user);

		void CreateContactRequest(Guid fromUID, Guid toUID);

		void CancelContactRequest(int requestId);

		void ConfirmContactRequest(int requestId);

		ContactRequest GetContactRequest(int requestId);
	}
}
