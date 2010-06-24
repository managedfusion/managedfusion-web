using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ManagedFusion
{
	public static class Utility
	{
		/// <summary>
		/// Parses the specified value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static T ParseEnum<T>(string value)
		{
			if (!typeof(T).IsEnum)
				throw new ArgumentException(typeof(T) + " is not an Enum");

			try
			{
				return (T)Enum.Parse(typeof(T), value, true);
			}
			catch (Exception)
			{
				return default(T);
			}
		}
	}
}
