window.ZokuChat = window.ZokuChat || {

	AcceptRequestButtonClick: function (clickedButton, id) {
		// Change button to indicate request accepted
		clickedButton.removeClass('btn-primary')
			.addClass('btn-success')
			.html('Request Accepted');

		// Disable both accept and cancel buttons
		$(`.request-${id}`).attr('disabled', 'disabled');

		// Send ajax request to ContactRequestController
		window.ZokuChat.SendAjaxRequest(`/ContactRequests/Accept?requestId=${id}`);
	},

	CancelRequestButtonClick: function (clickedButton, id) {
		// Change button to indicate request cancelled
		clickedButton.html('Request Declined');

		// Disable both accept and cancel buttons
		$(`.request-${id}`).attr('disabled', 'disabled');

		// Send ajax request to ContactRequestController
		window.ZokuChat.SendAjaxRequest(`/ContactRequests/Cancel?requestId=${id}`);
	},

	SendRequestButtonClick: function (clickedButton, id) {
		// Change button to indicate request sent
		clickedButton.removeClass('btn-primary')
			.addClass('btn-success')
			.html('Request Sent')
			.attr('disabled', 'disabled');

		// Send ajax request to ContactRequestController
		window.ZokuChat.SendAjaxRequest(`/ContactRequests/Create?requestedUID=${id}`);
	},

	BlockUserButtonClick: function (clickedButton, id) {
		// Change button to indicate user blocked
		clickedButton.html('User Blocked')
			.attr('disabled', 'disabled');

		// Disable other buttons
		clickedButton.siblings('button').attr('disabled', 'disabled');

		// Send ajax request to BlockedUserController
		window.ZokuChat.SendAjaxRequest(`/BlockedUsers/Block?blockedUID=${id}`);
	},

	UnblockUserButtonClick: function (clickedButton, id) {
		// Change button to indicate user unblocked
		clickedButton.removeClass('btn-primary')
			.addClass('btn-success')
			.html('User Unblocked')
			.attr('disabled', 'disabled');

		// Send ajax request to BlockedUserController
		window.ZokuChat.SendAjaxRequest(`/BlockedUsers/Unblock?blockedUID=${id}`);
	},

	RemoveContactButtonClick: function (clickedButton, id) {
		// Change button to indicate contact removed
		clickedButton.removeClass('btn-primary')
			.addClass('btn-success')
			.html('Contact Removed')
			.attr('disabled', 'disabled');

		// Send ajax request to ContactController
		window.ZokuChat.SendAjaxRequest(`/Contacts/Remove?contactId=${id}`);
	},

	SendAjaxRequest: function (url, successCallback, errorCallback) {
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
	},

	GetLocalDateStr: function (date) {
		return `${1 + date.getMonth()}/${date.getDate()}/${date.getFullYear()}`;
	},

	GetLocalTimeStr: function (date) {
		return `${date.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true })}`;
	},

	GetLocalDateTimeStr: function (date) {
		return `${window.ZokuChat.GetLocalDateStr(date)} at ${window.ZokuChat.GetLocalTimeStr(date)}`;
	}

};