using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewComponents
{
    public class SiteHeaderViewComponent : ViewComponent
    {
        private readonly DbContext db;

        public SiteHeaderViewComponent()//DbContext context
        {
            //db = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // This will need to use await keyword when we retrieve items from db
            return View();
        }
    }
}