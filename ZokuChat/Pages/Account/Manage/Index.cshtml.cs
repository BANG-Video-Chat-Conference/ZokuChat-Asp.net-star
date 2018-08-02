using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZokuChat.Data;

namespace ZokuChat.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
		public ZokuChatUser CurrentUser;

        public async Task OnGetAsync(UserManager<ZokuChatUser> userManager)
        {
			CurrentUser = await userManager.GetUserAsync(User);
        }
    }
}