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

		void DeleteRoom(User actionUser, Room room);

		void AddMessage(User actionUser, Room room, string text);

		Message GetMessage(int messageId);

		void DeleteMessage(User actionUser, Message message);
    }
}
