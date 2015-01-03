using System.Text;
using System.Collections.Generic;
using System;


namespace MasDev.Extensions
{
	public static class StringBuilderExtensions
	{
		public static void AppendForEach<T> (this StringBuilder sb, ICollection<T> ienum, Func<T, string> toString, string separator)
		{
			var count = ienum.Count-1;
			ienum.ForEach ((pos, element) => {
				sb.Append (toString (element));
				if (pos  != count)
					sb.Append (separator);
			});
		}
	}
}

