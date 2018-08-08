window.ZokuChat = window.ZokuChat || {};

window.ZokuChat.AcceptRequestButtonClick = function (clickedButton, id) {
	// Change button to indicate request sent
	clickedButton.removeClass('btn-primary')
	.addClass('btn-success')
	.html('Request Accepted')
	.attr('disabled', 'disabled');

	// Send ajax request to ContactRequestController
	window.ZokuChat.SendAjaxRequest(`/ContactRequests/Accept?requestId=${id}`);
};

window.ZokuChat.CancelRequestButtonClick = function (clickedButton, id) {
	// Change button to indicate request sent
	clickedButton.removeClass('btn-danger')
		.addClass('btn-success')
		.html('Request Accepted')
		.attr('disabled', 'disabled');

	// Send ajax request to ContactRequestController
	window.ZokuChat.SendAjaxRequest(`/ContactRequests/Cancel?requestId=${id}`);
};

window.ZokuChat.SendRequestButtonClick = function (clickedButton, id) {
	// Change button to indicate request sent
	clickedButton.removeClass('btn-primary')
		.addClass('btn-success')
		.html('Request Sent')
		.attr('disabled', 'disabled');

	// Send ajax request to ContactRequestController
	window.ZokuChat.SendAjaxRequest(`/ContactRequests/Create?requestedUID=${id}`);
};

window.ZokuChat.BlockUserButtonClick = function (clickedButton, id) {
	// Change button to indicate request sent
	clickedButton.html('User Blocked')
		.attr('disabled', 'disabled');

	// Send ajax request to BlockedUserController
	window.ZokuChat.SendAjaxRequest(`/BlockedUsers/Block?blockedUID=${id}`);
};

window.ZokuChat.UnblockUserButtonClick = function (clickedButton, id) {
	// Change button to indicate request sent
	clickedButton.removeClass('btn-primary')
		.addClass('btn-success')
		.html('User Unblocked')
		.attr('disabled', 'disabled');

	// Send ajax request to BlockedUserController
	window.ZokuChat.SendAjaxRequest(`/BlockedUsers/Unblock?blockedUID=${id}`);
};

window.ZokuChat.RemoveContactButtonClick = function (clickedButton, id) {
	// Change button to indicate request sent
	clickedButton.removeClass('btn-primary')
		.addClass('btn-success')
		.html('Contact Removed')
		.attr('disabled', 'disabled');

	// Send ajax request to ContactController
	window.ZokuChat.SendAjaxRequest(`/Contacts/Remove?contactId=${id}`);
};

window.ZokuChat.SendAjaxRequest = function (url, successCallback, errorCallback) {
	$.ajax({
		url: url,
		dataType: 'json'
	}).done(function (data) {
		if (data.IsSuccessful) {
			if (successCallback) {
				successCallback();
			}
		} else {
			if (errorCallback) {
				errorCallback();
			}
		}
	});
};