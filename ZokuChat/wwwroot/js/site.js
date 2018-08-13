window.ZokuChat = window.ZokuChat || {

	acceptRequestButtonClick: function (clickedButton, id) {
		// Change button to indicate request accepted
		clickedButton.removeClass('btn-primary')
			.addClass('btn-success')
			.html('Request Accepted');

		// Disable both accept and cancel buttons
		$(`.request-${id}`).attr('disabled', 'disabled');

		// Send ajax request to ContactRequestController
		window.ZokuChat.sendAjaxRequest(`/ContactRequests/Accept?requestId=${id}`);
	},

	cancelRequestButtonClick: function (clickedButton, id) {
		// Change button to indicate request cancelled
		clickedButton.html('Request Declined');

		// Disable both accept and cancel buttons
		$(`.request-${id}`).attr('disabled', 'disabled');

		// Send ajax request to ContactRequestController
		window.ZokuChat.sendAjaxRequest(`/ContactRequests/Cancel?requestId=${id}`);
	},

	sendRequestButtonClick: function (clickedButton, id) {
		// Change button to indicate request sent
		clickedButton.removeClass('btn-primary')
			.addClass('btn-success')
			.html('Request Sent')
			.attr('disabled', 'disabled');

		// Send ajax request to ContactRequestController
		window.ZokuChat.sendAjaxRequest(`/ContactRequests/Create?requestedUID=${id}`);
	},

	blockUserButtonClick: function (clickedButton, id) {
		if (confirm('Are you sure you want to block this user? This action can be undone in Manage Account.')) {
			// Change button to indicate user blocked
			clickedButton.html('User Blocked')
				.attr('disabled', 'disabled');

			// Disable other buttons
			clickedButton.siblings('button').attr('disabled', 'disabled');

			// Send ajax request to BlockedUserController
			window.ZokuChat.sendAjaxRequest(`/BlockedUsers/Block?blockedUID=${id}`);
		}
	},

	unblockUserButtonClick: function (clickedButton, id) {
		// Change button to indicate user unblocked
		clickedButton.removeClass('btn-primary')
			.addClass('btn-success')
			.html('User Unblocked')
			.attr('disabled', 'disabled');

		// Send ajax request to BlockedUserController
		window.ZokuChat.sendAjaxRequest(`/BlockedUsers/Unblock?blockedUID=${id}`);
	},

	removeContactButtonClick: function (clickedButton, id) {
		// Change button to indicate contact removed
		clickedButton.removeClass('btn-primary')
			.addClass('btn-success')
			.html('Contact Removed')
			.attr('disabled', 'disabled');

		// Send ajax request to ContactController
		window.ZokuChat.sendAjaxRequest(`/Contacts/Remove?contactId=${id}`);
	},

	deleteRoom: function (id) {
		if (confirm('Are you sure you want to delete this room? This action cannot be undone.')) {
			// Redirect to RoomController
			window.location = `/Rooms/Delete?roomId=${id}`;
		}
	},

	sendAjaxRequest: function (url, successCallback, errorCallback) {
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

	getLocalDateStr: function (date) {
		return `${1 + date.getMonth()}/${date.getDate()}/${date.getFullYear()}`;
	},

	getLocalTimeStr: function (date) {
		return `${date.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true })}`;
	},

	getLocalDateTimeStr: function (date) {
		return `${window.ZokuChat.getLocalDateStr(date)} at ${window.ZokuChat.getLocalTimeStr(date)}`;
	},

	initContactsListSelect2: function (selector, placeholderText) {
		return selector.select2({
			ajax: {
				url: '/Contacts/List',
				data: function (params) {
					return { searchTerm: params.term };
				},
				dataType: 'json',
				delay: 250,
				processResults: function (data) {
					let results = [];
					let contacts = data.contacts;

					for (let x = 0; x < contacts.length; x++) {
						let contact = contacts[x];
						results.push({ id: contact.id, text: contact.userName });
					}

					return {
						results: results
					};
				}
			},
			multiple: true,
			placeholder: placeholderText
		});
	}

};