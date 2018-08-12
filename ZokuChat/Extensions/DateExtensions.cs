using Microsoft.AspNetCore.Html;
using System;
using System.Globalization;

namespace ZokuChat.Extensions
{
    public static class DateExtensions
    {
		const string TIME_FORMAT = "h:mm tt";
		const string DATE_FORMAT = "M/d/yyyy";

		public static string ToZokuChatTimeString(this DateTime dateTime)
		{
			return dateTime.ToString(TIME_FORMAT, CultureInfo.InvariantCulture);
		}

		public static string ToZokuChatDateString(this DateTime dateTime)
		{
			return dateTime.ToString(DATE_FORMAT, CultureInfo.InvariantCulture);
		}

		public static string ToZokuChatDateTimeString(this DateTime dateTime)
		{
			return $"{dateTime.ToString(DATE_FORMAT, CultureInfo.InvariantCulture)} at {dateTime.ToString(TIME_FORMAT, CultureInfo.InvariantCulture)}";
		}

		public static string ToLocalDateTimeString(this DateTime dateTime)
		{
			return dateTime.SpecifyUtc().ToString("o");
		}

		public static DateTime SpecifyUtc(this DateTime dateTime)
		{
			return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
		}
	}
}
