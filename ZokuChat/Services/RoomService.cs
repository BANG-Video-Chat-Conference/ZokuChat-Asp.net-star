using FluentAssertions;
using System;
using System.Linq;
using ZokuChat.Data;
using ZokuChat.Models;

namespace ZokuChat.Services
{
	public class RoomService : IRoomService
	{
		private readonly Context _context;

		public RoomService(Context context)
		{
			_context = context;
		}

		public void CreateRoom(User actionUser, Room room)
		{
			// Validate
			actionUser.Should().NotBeNull();
			room.Should().NotBeNull();

			// Set default properties
			DateTime now = DateTime.UtcNow;

			room.CreatorUID = actionUser.Id;
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
			return _context.Rooms.Find(roomId);
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

			return _context.Rooms.Where(r => r.CreatorUID.Equals(user.Id));
		}
	}
}
