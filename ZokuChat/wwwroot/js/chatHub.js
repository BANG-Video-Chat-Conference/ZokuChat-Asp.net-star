var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();
var roomId;

connection.start().catch(function (err) {
	return console.error(err.toString());
});