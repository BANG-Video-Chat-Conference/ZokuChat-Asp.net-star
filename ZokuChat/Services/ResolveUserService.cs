using Microsoft.AspNetCore.Http;

namespace ZokuChat.Services
{
	public class ResolveUserService : IResolveUserService
	{
		private readonly IHttpContextAccessor _context;

		public ResolveUserService(IHttpContextAccessor context)
		{
			_context = context;
		}

		public string GetCurrentUserName()
		{
			return _context.HttpContext.User?.Identity?.Name;
		}
	}
}
