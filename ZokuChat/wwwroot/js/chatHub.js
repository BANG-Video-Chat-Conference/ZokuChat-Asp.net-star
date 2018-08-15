var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var roomId;
var creatorId;
var currentUserId;

connection.on("ReceiveMessage", function (userName, text) {
	
});

connection.on("ReceiveError", function (caption, message) {

});

connection.start().catch(function (err) {
	return console.error(err.toString());
}).then(function (value) {
	// Join the chat room group
	connection.invoke("JoinRoom", roomId).catch(function (err) {
		return console.error(err.toString());
	});
});