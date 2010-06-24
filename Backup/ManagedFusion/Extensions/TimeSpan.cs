using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
	public static class DateTimeExtensions
	{
		/// <summary>
		/// Toes the UTC time since string.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <returns></returns>
		public static string ToUtcTimeSinceString(this DateTime date)
		{
			return (DateTime.UtcNow - date).ToLongString();
		}

		/// <summary>
		/// Toes the time since string.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <returns></returns>
		public static string ToTimeSinceString(this DateTime date)
		{
			return (DateTime.Now - date).ToLongString();
		}

		/// <summary>
		/// Returns time span in a long string format.
		/// </summary>
		/// <param name="time">The time.</param>
		/// <returns></returns>
		public static string ToLongString(this TimeSpan time)
		{
			string output = String.Empty;

			if (time.Days > 0)
				output += time.Days + " days ";

			if ((time.Days == 0 || time.Days == 1) && time.Hours > 0)
				output += time.Hours + " hr ";

			if (time.Days == 0 && time.Minutes > 0)
				output += time.Minutes + " min ";

			if (output.Length == 0)
				output += time.Seconds + " sec";

			return output.Trim();
		}
	}
}
