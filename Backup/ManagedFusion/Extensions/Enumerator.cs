using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
	public static class EnumeratorExtensions
	{
		public static IEnumerable<TResult> ForEach<T1, TResult>(this IEnumerable<T1> enumerable, Func<T1, TResult> cast)
		{
			List<TResult> list = new List<TResult>();

			foreach (var e in enumerable)
				list.Add(cast(e));

			return list;
		}
	}
}
