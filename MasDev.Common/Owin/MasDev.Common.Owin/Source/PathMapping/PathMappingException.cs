using System;

namespace MasDev.Common.Owin
{
	public class PathMappingException :Exception
	{
		public PathMappingException (string format, params object[] args) : base (string.Format (format, args))
		{
			
		}
	}
}

