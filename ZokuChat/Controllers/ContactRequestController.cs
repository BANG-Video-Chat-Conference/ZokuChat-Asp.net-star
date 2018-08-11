using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZokuChat.Controllers.Responses;
using ZokuChat.Data;
using ZokuChat.Extensions;
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
		private readonly IContactService _contactService;
		private readonly IContactRequestService _contactRequestService;
		private readonly IExceptionService _exceptionService;

		public ContactRequestController(
			Context context,
			IUserService userService,
			IContactService contactService,
			IContactRequestService contactRequestService,
			IExceptionService exceptionService)
		{
			_context = context;
			_userService = userService;
			_contactService = contactService;
			_contactRequestService = contactRequestService;
			_exceptionService = exceptionService;
		}

        [Route("Create")]
        public JsonResult CreateRequest(string requestedUID)
        {
			GenericResponse result = new GenericResponse() { IsSuccessful = false };

			try
			{
				if (requestedUID.IsNullOrWhitespace())
				{
					result.ErrorMessage = "Bad request.";
					return new JsonResult(result);
				}

				// Retrieve the requested user
				User requested = _userService.GetUserByUID(requestedUID);

				if (requested != null && ContactRequestPermissionHelper.CanMakeContactRequest(_contactService, _contactRequestService, _context.CurrentUser, requested))
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
				result.ErrorMessage = "An exception occured.";
				_exceptionService.ReportException(e);
			}

			return new JsonResult(result);
        }

		[Route("Accept")]
		public JsonResult AcceptRequest(int requestId)
		{
			GenericResponse result = new GenericResponse() { IsSuccessful = false };

			try
			{
				if (requestId <= 0)
				{
					result.ErrorMessage = "Bad request.";
					return new JsonResult(result);
				}

				// Retrieve the requested user
				ContactRequest request = _contactRequestService.GetContactRequest(requestId);

				if (request != null && ContactRequestPermissionHelper.CanModifyContactRequest(_context.CurrentUser, request))
				{
					// Confirm the request
					_contactRequestService.ConfirmContactRequest(_context.CurrentUser, request);

					// If we got this far we're successful
					result.IsSuccessful = true;
				}
				else
				{
					result.ErrorMessage = "You do not have permission to modify this contact request.";
				}
			}
			catch (Exception e)
			{
				result.ErrorMessage = "An exception occured.";
				_exceptionService.ReportException(e);
			}

			return new JsonResult(result);
		}

		[Route("Cancel")]
		public JsonResult CancelRequest(int requestId)
		{
			GenericResponse result = new GenericResponse() { IsSuccessful = false };

			try
			{
				if (requestId <= 0)
				{
					result.ErrorMessage = "Bad request.";
					return new JsonResult(result);
				}

				// Retrieve the requested user
				ContactRequest request = _contactRequestService.GetContactRequest(requestId);

				if (request != null && ContactRequestPermissionHelper.CanModifyContactRequest(_context.CurrentUser, request))
				{
					// Cancel the request
					_contactRequestService.CancelContactRequest(_context.CurrentUser, request);

					// If we got this far we're successful
					result.IsSuccessful = true;
				}
				else
				{
					result.ErrorMessage = "You do not have permission to modify this contact request.";
				}
			}
			catch (Exception e)
			{
				result.ErrorMessage = "An exception occured.";
				_exceptionService.ReportException(e);
			}

			return new JsonResult(result);
		}
	}
}
