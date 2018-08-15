var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var roomId;
var creatorId;
var currentUserId;
var messagesContainer;

connection.on("ReceiveMessage", function (userName, userId, text) {
	let userNameCSS = userId == creatorId ? 'text-creator' : 'text-contact';
	messagesContainer.append(`<div><span class='${userNameCSS}'>${userName}</span>: ${text}</div>`);
});

connection.on("ReceiveError", function (caption, message) {
	messagesContainer.append(`<div><span class='text-error'>Error</span>: ${caption} - ${message}</div>`);
});

connection.start().catch(function (err) {
	return console.error(err.toString());
}).then(function (value) {
	// Join the chat room group
	connection.invoke("JoinRoom", roomId).catch(function (err) {
		return console.error(err.toString());
	});
});

function sendMessage(text) {
	connection.invoke("SendMessage", roomId, text).catch(function (err) {
		return console.error(err.toString());
	});
}