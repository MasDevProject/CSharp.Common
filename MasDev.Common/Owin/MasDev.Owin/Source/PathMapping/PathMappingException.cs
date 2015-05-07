using System;

namespace MasDev.Owin.PathMapping
{
	public class PathMappingException :Exception
	{
		public PathMappingException (string format, params object[] args) : base (string.Format (format, args))
		{
			
		}
	}
}

