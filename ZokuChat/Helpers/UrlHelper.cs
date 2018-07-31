namespace ZokuChat.Helpers
{
    public class UrlHelper
    {
		private const string _CONTACTS_URL = "/Contacts/List";
		private const string _LOGIN_URL = "/Identity/Account/Login";
		private const string _REGISTER_URL = "/Identity/Account/Register";
		private const string _HOME_URL = "/";
		private const string _ABOUT_URL = "/About";
		private const string _ERROR_URL = "/Error";

		public static string GetContactsListUrl()
		{
			return _CONTACTS_URL;
		}
    }
}
