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

class Broadcast {
	constructor(stream, userId) {
		this.stream = stream;
		this.userId = userId;
	}
}

window.ZokuChat.chat = {};
window.ZokuChat.chat.room = null;

var app = new Vue({
	el: '#chat-app',
	data: {
		window: window,
		connection: new signalR.HubConnectionBuilder().withUrl("/chatHub").build(),
		peerConnection: new RTCPeerConnection({
			"iceServers": [
				{ "urls": "stun:stun.1.google.com:19302" },
				{ "urls": "stun:stun1.l.google.com:19302" }
			]
		}),
		contacts: [],
		messages: [],
		broadcasting: false,
		broadcasts: [],
		errors: []
	},
	methods: {
		init: () => {
			// Initialize signalr handlers and connection
			app.connection.on("ReceiveMessages", function (messages) {
				messages.forEach(function (message) {
					app.messages.push(message);
				});

				// Scroll to latest
				app.$nextTick(() => {
					app.scrollToLatestMessage();
				});
			});

			app.connection.on("ReceiveContacts", function (contacts) {
				contacts.forEach(function (contact) {
					app.contacts.push(contact);
				});
			});

			app.connection.on("ReceiveMessage", function (message) {
				app.messages.push(message);

				// Scroll to latest
				app.$nextTick(() => {
					app.scrollToLatestMessage();
				});
			});

			app.connection.on("ReceiveDeleteMessage", function (message) {
				let index = app.messages.findIndex(function (m) {
					return m.id === message.id;
				});

				if (index > -1) app.messages.splice(index, 1, message);
			});

			app.connection.on("ReceiveBroadcast", function (broadcast) {
				app.broadcasts.push(broadcast);
			});

			app.connection.on("ReceiveDeleteBroadcast", function (broadcast) {
				let index = app.broadcasts.findIndex(function (b) {
					return b.userId === broadcast.userId;
				});

				if (index > -1) {
					// Delete the stream
					let tracks = app.broadcasts[index].stream.getTracks();
					tracks.forEach(function (track) {
						track.stop();
					});

					// Delete broadcast object
					app.broadcasts.splice(index, 1);
				}
			});

			app.connection.on("ReceiveError", function (caption, message) {
				app.errors.push(caption, message);
			});

			app.connection.start().then(function (value) {
					app.joinRoom().then(function (value) {
							app.retrieveMessages();
						})
						.catch(function (err) {
							return console.error(err.toString());
						});
				}).catch(function (err) {
					return console.error(err.toString());
				});

			// Setup rtc handlers
			app.peerConnection.ontrack = function (e) {
				//e.streams[0]
			};

			app.peerConnection.onicecandidate = function (e) {
				if (e.candidate) {
					app.sendCandidate(e.candidate);
				} 
			};

			app.connection.on("ReceiveOffer", function (offer) {
				app.peerConnection.setRemoteDescription(new RTCSessionDescription(offer));
				app.peerConnection.createAnswer(function (answer) {
					app.peerConnection.setLocalDescription(answer);
					app.sendAnswer(answer);
				}); 
			});

			app.connection.on("ReceiveAnswer", function (answer) {
				app.peerConnection.setRemoteDescription(new RTCSessionDescription(answer));
			});

			app.connection.on("ReceiveCandidate", function (candidate) {
				app.peerConnection.addIceCandidate(new RTCIceCandidate(candidate));
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
		sendAnswer: (answer) => {
			return app.connection.invoke("SendAnswer", window.ZokuChat.chat.room.id, JSON.stringify(answer)).catch(function (err) {
				return console.error(err.toString());
			});
		},
		sendCandidate: (candidate) => {
			return app.connection.invoke("SendCandidate", window.ZokuChat.chat.room.id, JSON.stringify(candidate)).catch(function (err) {
				return console.error(err.toString());
			});
		},
		deleteMessage: (message) => {
			return app.connection.invoke("DeleteMessage", window.ZokuChat.chat.room.id, message.id).catch(function (err) {
				return console.error(err.toString());
			});
		},
		canDeleteMessage: (message) => {
			let currentUserId = window.ZokuChat.chat.room.currentUserId;
			return currentUserId === message.userId || currentUserId === window.ZokuChat.chat.room.creatorId;
		},
		scrollToLatestMessage: () => {
			let container = $('#messages-container');
			container.scrollTop(container.height());
		},
		startBroadcast: () => {
			navigator.mediaDevices.getUserMedia({
				video: {
					width: { min: 640, ideal: 1920, max: 1920 },
					height: { min: 480, ideal: 1080, max: 1080 }
				},
				audio: true
			}).then(function (stream) {
				let broadcast = new Broadcast(stream, window.ZokuChat.chat.room.currentUserId);
				app.peerConnection.addStream(stream);
				app.broadcasts.push(broadcast);

				app.$nextTick(() => {
					document.querySelector(`video#broadcast-${window.ZokuChat.chat.room.currentUserId}`).srcObject = stream;
					app.broadcasting = true;
				});
			});
		},
		stopBroadcast: () => {
			return app.connection.invoke("StopBroadcast", window.ZokuChat.chat.room.id)
				.then(function () {
					app.broadcasting = false;
				})
				.catch(function (err) {
					return console.error(err.toString());
				});
		}
	}
});