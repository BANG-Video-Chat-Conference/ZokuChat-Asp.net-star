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
		contacts: [],
		messages: [],
		errors: []
	},
	methods: {
		init: () => {
			app.connection.on("ReceiveMessages", function (messages) {
				messages.forEach(function (message) {
					app.messages.push(message);
				});
			});

			app.connection.on("ReceiveContacts", function (contacts) {
				contacts.forEach(function (contact) {
					app.contacts.push(contact);
				});
			});

			app.connection.on("ReceiveMessage", function (message) {
				app.messages.push(message);
			});

			app.connection.on("ReceiveDeleteMessage", function (message) {
				let index = app.messages.findIndex(function (m) {
					return m.id === message.id;
				});

				if (index > -1) app.messages.splice(index, 1, message);
			});

			app.connection.on("ReceiveError", function (caption, message) {
				app.errors.push(caption, message);
			});

			app.connection.start().catch(function (err) {
				return console.error(err.toString());
			}).then(function (value) {
				app.joinRoom().then(function (value) {
					app.retrieveMessages();
				});
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
		},
		sendButtonClick: () => {
			if ($('#message-input').val().length > 0) {
				app.sendMessage($('#message-input').val());
				$('#message-input').val('');
			}
		},
		deleteMessage: (message) => {
			return app.connection.invoke("DeleteMessage", window.ZokuChat.chat.room.id, message.id).catch(function (err) {
				return console.error(err.toString());
			});
		},
		canDeleteMessage: (message) => {
			let currentUserId = window.ZokuChat.chat.room.currentUserId;
			return currentUserId === message.userId || currentUserId === window.ZokuChat.chat.room.creatorId;
		}
	}
});