using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ZokuChat.Data;
using ZokuChat.Services;

namespace ZokuChat.Models
{
    public class Context : IdentityDbContext<User>
    {
		private readonly IResolveUserService _resolveUserService;

		public DbSet<Contact> Contacts { get; set; }
		public DbSet<ContactRequest> ContactRequests { get; set; }
		public DbSet<BlockedUser> BlockedUsers { get; set; }
		public DbSet<Room> Rooms { get; set; }
		public DbSet<RoomContact> RoomContacts { get; set; }

		private User _currentUser;
		public User CurrentUser
		{
			get
			{
				if (_currentUser == null)
				{
					_currentUser = Users.Where(u => u.UserName.Equals(_resolveUserService.GetCurrentUserName())).FirstOrDefault();
				}

				return _currentUser;
			}
		}

		public Context(DbContextOptions<Context> options, IResolveUserService resolveUserService)
            : base(options)
        {
			_resolveUserService = resolveUserService;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

			builder.Entity<RoomContact>()
				.HasOne(rc => rc.Room)
				.WithMany(r => r.Contacts)
				.HasForeignKey(r => r.RoomId)
				.OnDelete(DeleteBehavior.Cascade);
		}
    }
}
