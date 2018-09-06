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
	constructor(streamId, userId) {
		this.streamId = streamId;
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
				let index = app.broadcasts.findIndex(function (b) {
					return b.stream.id === broadcast.streamId;
				});

				if (index > -1) {
					let foundBroadcast = app.broadcasts[index];

					foundBroadcast.streamId = broadcast.streamId;
					foundBroadcast.userId = broadcast.userId;

					app.$nextTick(() => {
						document.querySelector(`video#broadcast-${broadcast.userId}`).srcObject = foundBroadcast.stream;
					});
				}
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
				let broadcast = new Broadcast();
				broadcast.stream = e.streams[0];

				app.broadcasts.push(broadcast);
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
				stream.getTracks().forEach(track => app.peerConnection.addTrack(track, stream));
				app.peerConnection.createOffer({
					offerToReceiveAudio: 1,
					offerToReceiveVideo: 1
				})
				.then(desc => {
					return app.peerConnection.setLocalDescription(desc);
				})
				.then(() => {
					return app.connection.invoke("SendOffer", window.ZokuChat.chat.room.id, JSON.stringify(app.peerConnection.localDescription))
						.then(() => {
							let broadcast = new Broadcast();
							broadcast.stream = stream;
							app.broadcasts.push(broadcast);

							app.connection.invoke("StartBroadcast", window.ZokuChat.chat.room.id, new Broadcast(stream.id, window.ZokuChat.chat.room.currentUserId))
								.then(() => app.broadcasting = true);
						})
						.catch(err => console.error(err.toString()));
				});
			});
		},
		stopBroadcast: () => {
			return app.connection.invoke("StopBroadcast", window.ZokuChat.chat.room.id)
				.then(() => app.broadcasting = false)
				.catch(err => console.error(err.toString()));
		}
	}
});