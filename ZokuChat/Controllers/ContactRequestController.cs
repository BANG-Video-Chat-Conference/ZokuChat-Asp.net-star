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
	[Route("ContactRequests")]
	[Authorize]
	public class ContactRequestController : Controller
    {
		private readonly Context _context;
		private readonly IUserService _userService;
		private readonly IContactRequestService _contactRequestService;
		private readonly ContactRequestPermissionHelper _contactRequestPermissionHelper;

		public ContactRequestController(
			Context context,
			IUserService userService,
			IContactRequestService contactRequestService,
			ContactRequestPermissionHelper contactRequestPermissionHelper)
		{
			_context = context;
			_userService = userService;
			_contactRequestService = contactRequestService;
			_contactRequestPermissionHelper = contactRequestPermissionHelper;
		}

        [Route("Create")]
        public JsonResult Create(Guid requestedUID)
        {
			GenericResponse result = new GenericResponse() { IsSuccessful = false };

			try
			{
				// Validate
				requestedUID.Should().NotBeEmpty();

				// Retrieve the requested user
				User requested = _userService.GetUserByUID(requestedUID);

				if (requested != null && _contactRequestPermissionHelper.CanMakeContactRequest(_context.CurrentUser, requested))
				{
					// Create the contact request
					_contactRequestService.CreateContactRequest(_context.CurrentUser, requested);

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
