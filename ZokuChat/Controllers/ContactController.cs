using System;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZokuChat.Controllers.Responses;
using ZokuChat.Data;
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

		public ContactController(
			Context context,
			IUserService userService,
			IContactService contactService)
		{
			_context = context;
			_userService = userService;
			_contactService = contactService;
		}

        [Route("Remove")]
        public JsonResult RemoveContact(int contactId)
        {
			GenericResponse result = new GenericResponse() { IsSuccessful = false };

			try
			{
				// Validate
				contactId.Should().BeGreaterThan(0);

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
				// Something went wrong, log the exception's message
				result.ErrorMessage = e.Message;
			}

			return new JsonResult(result);
        }
	}
}
