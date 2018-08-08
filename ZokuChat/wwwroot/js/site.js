window.ZokuChat = window.ZokuChat || {};

window.ZokuChat.AcceptRequestButtonClick = function (clickedButton, id) {
	// Change button to indicate request sent
	clickedButton.html('Request Accepted');
	clickedButton.attr("disabled", "disabled");

	// Send ajax request to ContactRequestController
	
};


window.ZokuChat.SendRequestButtonClick = function (clickedButton, id) {
	// Change button to indicate request sent
	clickedButton.removeClass('btn-primary');
	clickedButton.addClass('btn-success');
	clickedButton.html('Request Sent');
	clickedButton.attr("disabled", "disabled");

	// Send ajax request to ContactRequestController
	
};

window.ZokuChat.BlockUserButtonClick = function (clickedButton, id) {
	// Change button to indicate request sent
	clickedButton.html('User Blocked');
	clickedButton.attr("disabled", "disabled");

	// Send ajax request to BlockedUserController

};

window.ZokuChat.UnblockUserButtonClick = function (clickedButton, id) {
	// Change button to indicate request sent
	clickedButton.html('User Unblocked');
	clickedButton.attr("disabled", "disabled");

	// Send ajax request to BlockedUserController
	
};

window.ZokuChat.RemoveContactButtonClick = function (clickedButton, id) {
	// Change button to indicate request sent
	clickedButton.removeClass('btn-primary');
	clickedButton.addClass('btn-success');
	clickedButton.html('Contact Removed');
	clickedButton.attr("disabled", "disabled");

	// Send ajax request to ContactController
	
};