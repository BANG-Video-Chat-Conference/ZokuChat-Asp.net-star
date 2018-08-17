class Room {
	constructor(id, creatorId, currentUserId) {
		this.id = id;
		this.creatorId = creatorId;
		this.currentUserId = currentUserId;
	}
}

class Message {
	constructor(userName, userId, text) {
		this.userName = userName;
		this.userId = userId;
		this.text = text;
	}
}

class Error {
	constructor(caption, message) {
		this.caption = caption;
		this.message = message;
	}
}

window.ZokuChat.chat = {};
window.ZokuChat.chat.room = null;

window.ZokuChat.chat.connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

window.ZokuChat.chat.connection.on("ReceiveMessages", function (messages) {
	messages.forEach(function (message) {
		window.ZokuChat.chat.app.messages.push(message);
	});
});

window.ZokuChat.chat.connection.on("ReceiveMessage", function (userName, userId, text) {
	window.ZokuChat.chat.app.messages.push(new Message(userName, userId, text));
});

window.ZokuChat.chat.connection.on("ReceiveError", function (caption, message) {
	window.ZokuChat.chat.app.errors.push(caption, message);
});

window.ZokuChat.chat.app = new Vue({
	el: '#chat-app',
	data: {
		window: window,
		messages: [],
		errors: []
	},
	methods: {
		startHub: function () {
			window.ZokuChat.chat.connection.start().catch(function (err) {
				return console.error(err.toString());
			}).then(function (value) {
				// Join the chat room group
				window.ZokuChat.chat.connection.invoke("JoinRoom", window.ZokuChat.chat.room.id).catch(function (err) {
					return console.error(err.toString());
				});
			}).then(function (value) {
				// Load the messages
				window.ZokuChat.chat.connection.invoke("GetMessages", window.ZokuChat.chat.room.id).catch(function (err) {
					return console.error(err.toString());
				});
			});
		},
		sendMessage: function (text) {
			window.ZokuChat.chat.connection.invoke("SendMessage", window.ZokuChat.chat.room.id, text).catch(function (err) {
				return console.error(err.toString());
			});
		}
	}
})