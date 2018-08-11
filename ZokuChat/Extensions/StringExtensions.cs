using System;

namespace ZokuChat.Extensions
{
    public static class StringExtensions
    {
		public static bool IsNullOrWhitespace(this string s)
		{
			return String.IsNullOrWhiteSpace(s);
		}
    }
}
