using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using ZokuChat.Data;
using ZokuChat.Models;

namespace ZokuChat.Services
{
	public class RoomService : IRoomService
	{
		private readonly Context _context;
		private readonly IContactService _contactService;

		public RoomService(Context context, IContactService contactService)
		{
			_context = context;
			_contactService = contactService;
		}

		public void CreateRoom(User actionUser, Room room)
		{
			// Validate
			actionUser.Should().NotBeNull();
			room.Should().NotBeNull();

			// Set default properties
			DateTime now = DateTime.UtcNow;

			room.CreatedUID = actionUser.Id;
			room.CreatedDateUtc = now;
			room.ModifiedUID = actionUser.Id;
			room.ModifiedDateUtc = now;
			room.IsDeleted = false;

			// Add to rooms and save
			_context.Rooms.Add(room);
			_context.SaveChanges();
		}

		public Room GetRoom(int roomId)
		{
			// Validate
			roomId.Should().BeGreaterThan(0);

			// Retrieve
			return _context.Rooms.Include(r => r.Contacts).Where(r => r.Id == roomId).FirstOrDefault();
		}

		public IQueryable<Room> GetRooms(RoomSearch search)
		{
			// Validate
			search.Should().NotBeNull();
			search.SearchText.Should().NotBeNullOrWhiteSpace();

			// Build query
			IQueryable<Room> query = _context.Rooms.Where(r => r.Name.Contains(search.SearchText) || r.Description.Contains(search.SearchText));

			if (search.IsDeleted.HasValue)
			{
				query = query.Where(r => r.IsDeleted == search.IsDeleted);
			}

			return query.Take(search.MaxResults);
		}

		public IQueryable<Room> GetRoomsForUser(User user)
		{
			// Validate
			user.Should().NotBeNull();

			// Build a query for room ids where user is a contact
			IQueryable<int> roomsWhereUserIsContact = _context.RoomContacts.Where(rc => rc.ContactUID.Equals(user.Id)).Select(rc => rc.RoomId);

			// Return rooms where user is the creator or a contact and are not deleted
			return _context.Rooms.Where(r => r.CreatedUID.Equals(user.Id) || roomsWhereUserIsContact.Contains(r.Id)).Where(r => !r.IsDeleted);
		}

		/// <summary>
		///	Adds UIDs to the room that are found in the user's contacts.
		/// </summary>
		public void AddRoomContacts(User actionUser, Room room, string[] UIDs)
		{
			// Validate
			actionUser.Should().NotBeNull();
			room.Should().NotBeNull();
			UIDs.Should().NotBeNullOrEmpty();

			// Filter out invalid UIDs
			IQueryable<string> contactUIDs =
				_contactService.GetUserContacts(actionUser)
					.Where(c => UIDs.Contains(c.ContactUID))
					.Select(c => c.ContactUID);

			// Create a list of room contacts
			DateTime now = DateTime.UtcNow;
			List<RoomContact> roomContacts = new List<RoomContact>();

			foreach (string contactUID in contactUIDs.Where(id => !room.Contacts.Any(c => c.ContactUID.Equals(id))))
			{
				roomContacts.Add(new RoomContact
				{
					ContactUID = contactUID,
					RoomId = room.Id,
					CreatedUID = actionUser.Id,
					CreatedDateUtc = now,
					ModifiedUID = actionUser.Id,
					ModifiedDateUtc = now
				});
			}

			// Save the list of room contacts
			_context.RoomContacts.AddRange(roomContacts);
			_context.SaveChanges();
		}

		/// <summary>
		///	Sets UIDs to the room that are found in the user's contacts.
		/// </summary>
		public void SetRoomContacts(User actionUser, Room room, string[] UIDs)
		{
			// Validate
			actionUser.Should().NotBeNull();
			room.Should().NotBeNull();
			UIDs.Should().NotBeNull();

			// Filter out invalid UIDs
			IQueryable<string> contactUIDs =
				_contactService.GetUserContacts(actionUser)
					.Where(c => UIDs.Contains(c.ContactUID))
					.Select(c => c.ContactUID);

			// Create a list of room contacts to add
			DateTime now = DateTime.UtcNow;
			List<RoomContact> roomContacts = new List<RoomContact>();

			foreach (string contactUID in contactUIDs.Where(id => !room.Contacts.Any(c => c.ContactUID.Equals(id))))
			{
				roomContacts.Add(new RoomContact
				{
					ContactUID = contactUID,
					RoomId = room.Id,
					CreatedUID = actionUser.Id,
					CreatedDateUtc = now,
					ModifiedUID = actionUser.Id,
					ModifiedDateUtc = now
				});
			}

			// Save the list of room contacts
			_context.RoomContacts.RemoveRange(_context.RoomContacts.Where(rc => rc.RoomId == room.Id && !UIDs.Contains(rc.ContactUID)));
			_context.RoomContacts.AddRange(roomContacts);
			_context.SaveChanges();
		}

		public void DeleteRoom(User actionUser, Room room)
		{
			// Validate
			actionUser.Should().NotBeNull();
			room.Should().NotBeNull();

			// Delete
			room.IsDeleted = true;
			room.ModifiedUID = actionUser.Id;
			room.ModifiedDateUtc = DateTime.UtcNow;

			// Save
			_context.SaveChanges();
		}

		public void AddMessage(User actionUser, Room room, string text)
		{
			// Validate
			actionUser.Should().NotBeNull();
			room.Should().NotBeNull();
			text.Should().NotBeNullOrWhiteSpace();

			// Add and save
			DateTime now = DateTime.UtcNow;

			_context.Messages.Add(new Message
			{
				RoomId = room.Id,
				Text = text,
				CreatedUID = actionUser.Id,
				CreatedDateUtc = now,
				ModifiedUID = actionUser.Id,
				ModifiedDateUtc = now,
				IsDeleted = false
			});

			_context.SaveChanges();
		}

		public void DeleteMessage(User actionUser, Message message)
		{
			// Validate
			actionUser.Should().NotBeNull();
			message.Should().NotBeNull();

			// Delete and save
			message.IsDeleted = true;
			message.ModifiedUID = actionUser.Id;
			message.ModifiedDateUtc = DateTime.UtcNow;

			_context.SaveChanges();
		}
	}
}
