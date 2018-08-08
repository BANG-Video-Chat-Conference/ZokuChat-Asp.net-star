using System;
using System.Globalization;

namespace ZokuChat.Extensions
{
    public static class DateExtensions
    {
		const string TIME_REGEX = "h:mm tt";
		const string DATE_REGEX = "d/M/yyyy";

		public static string ToZokuChatTimeString(this DateTime dateTime)
		{
			return dateTime.ToString(TIME_REGEX, CultureInfo.InvariantCulture);
		}

		public static string ToZokuChatDateString(this DateTime dateTime)
		{
			return dateTime.ToString(DATE_REGEX, CultureInfo.InvariantCulture);
		}

		public static string ToZokuChatDateTimeString(this DateTime dateTime)
		{
			return $"{dateTime.ToString(DATE_REGEX, CultureInfo.InvariantCulture)} at {dateTime.ToString(TIME_REGEX, CultureInfo.InvariantCulture)}";
		}
	}
}
