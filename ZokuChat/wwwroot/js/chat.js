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

const app = new Vue({
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
		myStreamId: -1,
		mySenders: [],
		streams: [],
		broadcasting: false,
		broadcasts: [],
		errors: []
	},
	methods: {
		init: function () {
			// Initialize signalr handlers and connection
			app.connection.on("ReceiveMessages", function (messages) {
				messages.forEach(function (message) {
					app.messages.push(message);
				});

				// Scroll to latest
				app.$nextTick(function () {
					app.scrollToLatestMessage();
				});
			});

			app.connection.on("ReceiveContact", function (contact) {
				if (app.broadcasting) {
					app.connection.invoke("StartBroadcast", window.ZokuChat.chat.room.id, new Broadcast(app.myStreamId, window.ZokuChat.chat.room.currentUserId))
						.then(() => app.sendOfferToUser(contact.userUID));
				}
			});

			app.connection.on("ReceiveContacts", function (contacts) {
				contacts.forEach(function (contact) {
					app.contacts.push(contact);
				});
			});

			app.connection.on("ReceiveMessage", function (message) {
				app.messages.push(message);

				// Scroll to latest
				app.$nextTick(function() {
					app.scrollToLatestMessage();
				});
			});

			app.connection.on("ReceiveDeleteMessage", function (message) {
				const index = app.messages.findIndex(function (m) {
					return m.id === message.id;
				});

				if (index > -1) app.messages.splice(index, 1, message);
			});

			app.connection.on("ReceiveBroadcast", function (broadcast) {
				if (app.broadcasts.findIndex((b) => b.userId === broadcast.userId) === -1) {
					app.broadcasts.push(new Broadcast(broadcast.streamId, broadcast.userId));
				}
			});

			app.connection.on("ReceiveDeleteBroadcast", function (broadcast) {
				const index = app.broadcasts.findIndex(function (b) {
					return b.userId === broadcast.userId;
				});

				if (index > -1) {
					// Delete the stream
					const streamIndex = app.streams.findIndex(function (s) {
						return s.id === app.broadcasts[index].streamId;
					});

					if (streamIndex > -1) {
						app.streams[streamIndex].getTracks().forEach(track => track.stop);
						app.streams.splice(streamIndex, 1);
					}

					// Delete broadcast object
					app.broadcasts.splice(index, 1);
				}
			});

			app.connection.on("ReceiveError", function (caption, message) {
				app.errors.push(caption, message);
			});

			app.connection.start()
				.then(function (value) {
					app.joinRoom().then(() => app.retrieveMessages());
				});

			// Setup rtc handlers
			app.peerConnection.ontrack = function (e) {
				if (app.streams.findIndex(s => s.id === e.streams[0].id) === -1) {
					const stream = e.streams[0];
					app.streams.push(stream);

					const broadcastIndex = app.broadcasts.findIndex(function (b) {
						return b.streamId === stream.id;
					});

					if (broadcastIndex > -1) {
						const broadcast = app.broadcasts[broadcastIndex];
						app.$nextTick(() => {
							document.querySelector(`video#broadcast-${broadcast.userId}`).srcObject = stream;
						});
					}
				}
			};

			app.peerConnection.onnegotiationneeded = function (e) {
				app.peerConnection.createOffer().then(desc => {
					return app.peerConnection.setLocalDescription(desc);
				}).then(() => app.sendOffer());
			};

			app.peerConnection.onicecandidate = function (e) {
				if (e.candidate) {
					app.sendCandidate(e.candidate);
				} 
			};

			app.connection.on("ReceiveOffer", function (offer, userId) {
				app.peerConnection.setRemoteDescription(JSON.parse(offer)).then(() => {
					app.peerConnection.createAnswer().then(answer => {
						app.peerConnection.setLocalDescription(answer);
						app.sendAnswer(answer, userId);
					}); 
				});
			});

			app.connection.on("ReceiveAnswer", function (answer, userId) {
				app.peerConnection.setRemoteDescription(JSON.parse(answer));
			});

			app.connection.on("ReceiveCandidate", function (candidate) {
				app.peerConnection.addIceCandidate(new RTCIceCandidate(JSON.parse(candidate)));
			});
		},
		joinRoom: function () {
			return app.connection.invoke("JoinRoom", window.ZokuChat.chat.room.id);
		},
		retrieveMessages: function () {
			return app.connection.invoke("GetMessages", window.ZokuChat.chat.room.id);
		},
		sendMessage: function (text) {
			return app.connection.invoke("SendMessage", window.ZokuChat.chat.room.id, text);
		},
		sendButtonClick: function () {
			const input = $('#message-input');
			if (input.val().length > 0) {
				app.sendMessage(input.val());
				input.val('');
			}
		},
		sendOffer: function () {
			return app.connection.invoke("SendOffer", window.ZokuChat.chat.room.id, JSON.stringify(app.peerConnection.localDescription));
		},
		sendOfferToUser: function (userId) {
			return app.connection.invoke("SendOfferToUser", window.ZokuChat.chat.room.id, userId, JSON.stringify(app.peerConnection.localDescription));
		},
		sendAnswer: function (answer, userId) {
			return app.connection.invoke("SendAnswer", window.ZokuChat.chat.room.id, userId, JSON.stringify(answer));
		},
		sendCandidate: function (candidate) {
			return app.connection.invoke("SendCandidate", window.ZokuChat.chat.room.id, JSON.stringify(candidate));
		},
		deleteMessage: function (message) {
			return app.connection.invoke("DeleteMessage", window.ZokuChat.chat.room.id, message.id);
		},
		canDeleteMessage: function (message) {
			const currentUserId = window.ZokuChat.chat.room.currentUserId;
			return currentUserId === message.userId || currentUserId === window.ZokuChat.chat.room.creatorId;
		},
		scrollToLatestMessage: function () {
			const container = $('#messages-container');
			container.scrollTop(container.prop("scrollHeight"));
		},
		startBroadcast: function () {
			navigator.mediaDevices.getUserMedia({
				video: {
					width: { min: 640, ideal: 1920, max: 1920 },
					height: { min: 480, ideal: 1080, max: 1080 }
				},
				audio: true
			}).then(function (stream) {
				app.myStreamId = stream.id;

				const broadcast = new Broadcast(stream.id, window.ZokuChat.chat.room.currentUserId);
				app.broadcasts.push(broadcast);

				app.$nextTick(() => {
					document.querySelector(`video#broadcast-${broadcast.userId}`).srcObject = stream;
				});

				app.connection.invoke("StartBroadcast", window.ZokuChat.chat.room.id, new Broadcast(app.myStreamId, window.ZokuChat.chat.room.currentUserId))
					.then(() => {
						app.broadcasting = true;
						app.streams.push(stream);
						stream.getTracks().forEach(track => app.mySenders.push(app.peerConnection.addTrack(track, stream)));
					});
			}).catch(function (err) {
				logError(err);
			});
		},
		stopBroadcast: function () {
			if (app.mySenders.length > 0) {
				app.mySenders.forEach(sender => app.peerConnection.removeTrack(sender));
				app.mySenders.splice(0, app.mySenders.length);
			}

			return app.connection.invoke("StopBroadcast", window.ZokuChat.chat.room.id)
				.then(() => app.broadcasting = false);
		},
		logError: function (err) {
			console.log(err.name + ": " + err.message);
		}
	}
});