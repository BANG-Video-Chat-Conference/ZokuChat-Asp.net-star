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
	[Route("BlockedUsers")]
	[Authorize]
	public class BlockedUserController : Controller
    {
		private readonly Context _context;
		private readonly IUserService _userService;
		private readonly IBlockedUserService _blockedUserService;
		private readonly BlockedUserPermissionHelper _blockedUserPermissionHelper;

		public BlockedUserController(
			Context context,
			IUserService userService,
			IBlockedUserService blockedUserService,
			BlockedUserPermissionHelper blockedUserPermissionHelper)
		{
			_context = context;
			_userService = userService;
			_blockedUserService = blockedUserService;
			_blockedUserPermissionHelper = blockedUserPermissionHelper;
		}

        [Route("Block")]
        public JsonResult BlockUser(Guid blockedUID)
        {
			GenericResponse result = new GenericResponse() { IsSuccessful = false };

			try
			{
				// Validate
				blockedUID.Should().NotBeEmpty();

				// Retrieve the requested user
				User blocked = _userService.GetUserByUID(blockedUID);

				if (blocked != null && _blockedUserPermissionHelper.CanBlockUser(_context.CurrentUser, blocked))
				{
					// Block the user
					_blockedUserService.BlockUser(_context.CurrentUser, blocked);

					// If we got this far we're successful
					result.IsSuccessful = true;
				}
				else
				{
					result.ErrorMessage = "You do not have permission to block this user.";
				}
			}
			catch (Exception e)
			{
				// Something went wrong, log the exception's message
				result.ErrorMessage = e.Message;
			}

			return new JsonResult(result);
        }

		[Route("Unblock")]
		public JsonResult UnblockUser(Guid blockedUID)
		{
			GenericResponse result = new GenericResponse() { IsSuccessful = false };

			try
			{
				// Validate
				blockedUID.Should().NotBeEmpty();

				// Retrieve the requested user
				User blocked = _userService.GetUserByUID(blockedUID);

				if (blocked != null && _blockedUserPermissionHelper.CanUnblockUser(_context.CurrentUser, blocked))
				{
					// Ublock the user
					_blockedUserService.UnblockUser(_context.CurrentUser, blocked);

					// If we got this far we're successful
					result.IsSuccessful = true;
				}
				else
				{
					result.ErrorMessage = "You do not have permission to unblock this user.";
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
