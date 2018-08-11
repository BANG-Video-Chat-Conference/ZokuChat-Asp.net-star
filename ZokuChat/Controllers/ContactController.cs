using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZokuChat.Controllers.Responses;
using ZokuChat.Helpers;
using ZokuChat.Models;
using ZokuChat.Services;

namespace ZokuChat.Controllers
{
	[Route("Contacts")]
	[Authorize]
	public class ContactController : Controller
    {
		private readonly Context _context;
		private readonly IUserService _userService;
		private readonly IContactService _contactService;
		private readonly IExceptionService _exceptionService;

		public ContactController(
			Context context,
			IUserService userService,
			IContactService contactService,
			IExceptionService exceptionService)
		{
			_context = context;
			_userService = userService;
			_contactService = contactService;
			_exceptionService = exceptionService;
		}

		[Route("List")]
		public JsonResult GetContacts()
		{
			ContactsResponse result = new ContactsResponse() { IsSuccessful = false };

			try
			{
				string[] contactUIDs = _contactService.GetUserContacts(_context.CurrentUser).Select(c => c.ContactUID).ToArray();

				if (contactUIDs.Length > 0)
				{
					result.Contacts = _userService.GetUserByUID(contactUIDs).Select(u => new ContactResult { Id = u.Id, UserName = u.UserName }).ToArray();
				}
				else
				{
					result.Contacts = new ContactResult[] {};
				}

				// If we got this far we're successful
				result.IsSuccessful = true;
			}
			catch (Exception e)
			{
				result.ErrorMessage = "An exception occurred";
				_exceptionService.ReportException(e);
			}

			return new JsonResult(result);
		}

        [Route("Remove")]
        public JsonResult RemoveContact(int contactId)
        {
			GenericResponse result = new GenericResponse() { IsSuccessful = false };

			try
			{
				if (contactId <= 0)
				{
					result.ErrorMessage = "Bad request.";
					return new JsonResult(result);
				}

				// Retrieve the requested user
				Contact contact = _contactService.GetContact(contactId);

				if (contact != null && ContactPermissionHelper.CanDeleteContact(_context.CurrentUser, contact))
				{
					// Delete the contact request
					_contactService.DeleteContact(contact);

					// If we got this far we're successful
					result.IsSuccessful = true;
				}
				else
				{
					result.ErrorMessage = "You do not have permission to contact request this user.";
				}
			}
			catch (Exception e)
			{
				result.ErrorMessage = "An exception occurred.";
				_exceptionService.ReportException(e);
			}

			return new JsonResult(result);
        }
	}
}
