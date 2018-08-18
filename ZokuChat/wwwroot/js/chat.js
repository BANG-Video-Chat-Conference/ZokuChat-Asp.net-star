class Room {
	constructor(id, creatorId, currentUserId) {
		this.id = id;
		this.creatorId = creatorId;
		this.currentUserId = currentUserId;
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

var app = new Vue({
	el: '#chat-app',
	data: {
		window: window,
		connection: new signalR.HubConnectionBuilder().withUrl("/chatHub").build(),
		messages: [],
		errors: []
	},
	methods: {
		init: () => {
			app.connection.start().catch(function (err) {
				return console.error(err.toString());
			}).then(function (value) {
				app.joinRoom().then(function (value) {
					app.retrieveMessages();
				});
			});

			app.connection.on("ReceiveMessages", function (messages) {
				messages.forEach(function (message) {
					app.messages.push(message);
				});
			});

			app.connection.on("ReceiveMessage", function (message) {
				app.messages.push(message);
			});

			app.connection.on("ReceiveError", function (caption, message) {
				app.errors.push(caption, message);
			});
		},
		joinRoom: () => {
			return app.connection.invoke("JoinRoom", window.ZokuChat.chat.room.id).catch(function (err) {
				return console.error(err.toString());
			});
		},
		retrieveMessages: () => {
			return app.connection.invoke("GetMessages", window.ZokuChat.chat.room.id).catch(function (err) {
				return console.error(err.toString());
			});
		},
		sendMessage: (text) => {
			return app.connection.invoke("SendMessage", window.ZokuChat.chat.room.id, text).catch(function (err) {
				return console.error(err.toString());
			});
		}
	}
})