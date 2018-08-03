using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using ZokuChat.Data;
using ZokuChat.Services;

namespace ZokuChat.Models
{
    public class ZokuChatContext : IdentityDbContext<ZokuChatUser>
    {
		private readonly IResolveUserService _resolveUserService;

		public DbSet<Contact> Contacts { get; set; }

		private ZokuChatUser _currentUser;
		public ZokuChatUser CurrentUser
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

		public ZokuChatContext(DbContextOptions<ZokuChatContext> options, IResolveUserService resolveUserService)
            : base(options)
        {
			_resolveUserService = resolveUserService;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
