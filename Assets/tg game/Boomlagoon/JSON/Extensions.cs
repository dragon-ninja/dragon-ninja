using System.Collections.Generic;

namespace Boomlagoon.JSON
{
	public static class Extensions
	{
		public static T Pop<T>(this List<T> list)
		{
			T result = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
			return result;
		}
	}
}
