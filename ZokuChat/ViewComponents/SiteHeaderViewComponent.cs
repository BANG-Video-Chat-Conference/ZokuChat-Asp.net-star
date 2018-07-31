using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZokuChat.Data;
using ZokuChat.Models;

namespace ViewComponents
{
	public class SiteHeaderViewComponent : ViewComponent
	{
		private readonly ZokuChatContext db;

		public SiteHeaderViewComponent(ZokuChatContext context)
		{
			db = context;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			// This will need to use await keyword when we retrieve items from db
			return View();
		}
	}
}