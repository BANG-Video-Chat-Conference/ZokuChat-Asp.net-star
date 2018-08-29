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
	constructor(userId, streamUrl) {
		this.userId = userId;
		this.streamUrl = streamUrl;
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
				{ "urls": "stun:stun1.l.google.com:19302" },
				{ "urls": "stun:stun2.l.google.com:19302" },
				{ "urls": "stun:stun3.1.google.com:19302" },
				{ "urls": "stun:stun4.1.google.com:19302" },
				{ "urls": "stun:stun01.sipphone.com" },
				{ "urls": "stun:stun.ekiga.net" },
				{ "urls": "stun:stun.fwdnet.net" },
				{ "urls": "stun:stun.ideasip.com" },
				{ "urls": "stun:stun.iptel.org" },
				{ "urls": "stun:stun.rixtelecom.se" },
				{ "urls": "stun:stun.schlund.de" },
				{ "urls": "stun:stunserver.org" },
				{ "urls": "stun:stun.softjoys.com" },
				{ "urls": "stun:stun.voiparound.com" },
				{ "urls": "stun:stun.voipbuster.com" },
				{ "urls": "stun:stun.voipstunt.com" },
				{ "urls": "stun:stun.voxgratia.org" },
				{ "urls": "stun:stun.xten.com" }
			]
		}),
		contacts: [],
		messages: [],
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
		startBroadcast: () => {
			let broadcast;

			navigator.getUserMedia({ video: true, audio: true }, function (stream) {
				broadcast = new Broadcast(window.ZokuChat.chat.room.currentUserId, window.URL.createObjectURL(stream));
				app.broadcasts.push(broadcast);
				app.peerConnection.addStream(stream);
			});

			if (broadcast) {
				return app.connection.invoke("StartBroadcast", window.ZokuChat.chat.room.id, broadcast).catch(function (err) {
					return console.error(err.toString());
				});
			}
		},
		stopBroadcast: () => {
			return app.connection.invoke("StopBroadcast", window.ZokuChat.chat.room.id).catch(function (err) {
				return console.error(err.toString());
			});
		}
	}
});