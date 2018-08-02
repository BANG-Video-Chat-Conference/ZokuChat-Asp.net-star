using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZokuChat.Data;

namespace ZokuChat.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
		private readonly UserManager<ZokuChatUser> _userManager;

		public ZokuChatUser CurrentUser;

		public IndexModel(UserManager<ZokuChatUser> userManager)
		{
			_userManager = userManager;
		}

        public async void OnGetAsync()
        {
			CurrentUser = await _userManager.GetUserAsync(User);
        }
    }
}