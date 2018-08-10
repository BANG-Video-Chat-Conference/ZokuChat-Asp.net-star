namespace ZokuChat.Helpers
{
    public class UrlHelper
    {
		private const string _CONTACTS_URL = "/Chat/Contact/Index";
		private const string _CONTACT_REQUESTS_URL = "/Chat/Request/Index";
		private const string _USER_SEARCH_URL = "/Chat/Contact/UserSearch";
		private const string _ROOMS_URL = "/Chat/Room/Index";
		private const string _NEW_ROOM_URL = "/Chat/Room/Create";
		private const string _LOGIN_URL = "/Account/Login";
		private const string _LOGOUT_URL = "/Account/Logout";
		private const string _CONFIRM_EMAIL_URL = "/Account/ConfirmEmail";
		private const string _MANAGE_URL = "/Account/Manage";
		private const string _BLOCKED_USERS_URL = "/Account/Manage/BlockedUsers";
		private const string _REGISTER_URL = "/Account/Register";
		private const string _HOME_URL = "/";
		private const string _ABOUT_URL = "/About";
		private const string _ERROR_URL = "/Error";
		private const string _ACCESS_DENIED_URL = "/Account/AccessDenied";
		private const string _VIEW_USER_URL = "/Chat/Contact/View";

		public static string GetContactsUrl()
		{
			return _CONTACTS_URL;
		}

		public static string GetContactRequestsUrl()
		{
			return _CONTACT_REQUESTS_URL;
		}

		public static string GetUserSearchUrl()
		{
			return _USER_SEARCH_URL;
		}

		public static string GetRoomsUrl()
		{
			return _ROOMS_URL;
		}

		public static string GetNewRoomUrl()
		{
			return _NEW_ROOM_URL;
		}

		public static string GetAboutUrl()
		{
			return _ABOUT_URL;
		}

		public static string GetErrorUrl()
		{
			return _ERROR_URL;
		}

		public static string GetHomeUrl()
		{
			return _HOME_URL;
		}

		public static string GetLoginUrl()
		{
			return _LOGIN_URL;
		}

		public static string GetLogoutUrl()
		{
			return _LOGOUT_URL;
		}

		public static string GetConfirmEmailUrl()
		{
			return _CONFIRM_EMAIL_URL;
		}

		public static string GetRegisterUrl()
		{
			return _REGISTER_URL;
		}

		public static string GetManageAccountUrl()
		{
			return _MANAGE_URL;
		}

		public static string GetManageBlockedUsersUrl()
		{
			return _BLOCKED_USERS_URL;
		}

		public static string GetAccessDeniedUrl()
		{
			return _ACCESS_DENIED_URL;
		}

		public static string GetViewUserUrl(string UID)
		{
			return $"{_VIEW_USER_URL}?userId={UID}";
		}
	}
}
