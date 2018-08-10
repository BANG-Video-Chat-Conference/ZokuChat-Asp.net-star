using FluentAssertions;
using System.Linq;
using ZokuChat.Data;
using ZokuChat.Models;
using ZokuChat.Services;

namespace ZokuChat.Helpers
{
    public class RoomPermissionHelper
    {
		public static bool CanAddRoomContact(User user, Room room)
		{
			// Validate
			user.Should().NotBeNull();
			room.Should().NotBeNull();

			// The user can add the contact if they are the room creator
			return IsRoomCreator(user, room);
		}

		public static bool CanViewRoom(IRoomService roomService, User user, Room room)
		{
			// Validate
			roomService.Should().NotBeNull();
			user.Should().NotBeNull();
			room.Should().NotBeNull();

			if (IsRoomCreator(user, room))
			{
				// Creators can always view their own rooms
				return true;
			}
			else
			{
				// The user isn't the creator so return true if they are a room contact
				return roomService.GetRoomContacts(room).Any(rc => rc.ContactUID.Equals(user.Id));
			}
		}

		public static bool IsRoomCreator(User user, Room room)
		{
			// Validate
			user.Should().NotBeNull();
			room.Should().NotBeNull();

			return room.CreatorUID.Equals(user.Id);
		}
    }
}
