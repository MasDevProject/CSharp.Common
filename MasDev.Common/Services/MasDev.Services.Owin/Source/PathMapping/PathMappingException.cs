using System;

namespace MasDev.Services
{
	public class PathMappingException :Exception
	{
		public PathMappingException (string format, params object[] args) : base (string.Format (format, args))
		{
			
		}
	}
}

