using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZokuChat.Data;

namespace ViewComponents
{
	public class SiteHeaderViewComponent : ViewComponent
	{
		private readonly ApplicationDbContext db;

		public SiteHeaderViewComponent(ApplicationDbContext context)
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