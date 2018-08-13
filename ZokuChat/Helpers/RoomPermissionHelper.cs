using FluentAssertions;
using System.Linq;
using ZokuChat.Data;
using ZokuChat.Models;

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

		public static bool CanViewRoom(User user, Room room)
		{
			// Validate
			user.Should().NotBeNull();
			room.Should().NotBeNull();

			if (room.IsDeleted)
			{
				// Deleted rooms cannot be viewed
				return false;
			}
			else if (IsRoomCreator(user, room))
			{
				// Creators can view their own rooms
				return true;
			}
			else
			{
				// The user isn't the creator so return true if they are a room contact
				return room.Contacts.Any(rc => rc.ContactUID.Equals(user.Id));
			}
		}

		public static bool CanEditRoom(User user, Room room)
		{
			// Validate
			user.Should().NotBeNull();
			room.Should().NotBeNull();

			if (!room.IsDeleted && IsRoomCreator(user, room))
			{
				// Creators can edit their own rooms if they are not deleted
				return true;
			}
			else
			{
				return false;
			}
		}

		private static bool IsRoomCreator(User user, Room room)
		{
			return room.CreatedUID.Equals(user.Id);
		}

		public static bool CanDeleteRoom(User user, Room room)
		{
			// Validate
			user.Should().NotBeNull();
			room.Should().NotBeNull();

			return IsRoomCreator(user, room) && !room.IsDeleted;
		}
    }
}
