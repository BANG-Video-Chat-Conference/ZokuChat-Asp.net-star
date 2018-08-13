using System.Collections.Generic;
using System.Linq;
using ZokuChat.Data;
using ZokuChat.Models;

namespace ZokuChat.Services
{
    public interface IRoomService
    {
		Room GetRoom(int roomId);

		IQueryable<Room> GetRooms(RoomSearch search);

		IQueryable<Room> GetRoomsForUser(User user);

		void CreateRoom(User actionUser, Room room);

		void AddRoomContacts(User actionUser, Room room, string[] UIDs);

		void SetRoomContacts(User actionUser, Room room, string[] UIDs);

		void DeleteRoom(Room room);
    }
}
