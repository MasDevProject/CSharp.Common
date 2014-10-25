using System;

namespace PizzApp.Server.Utils
{
	public static class GuidGenerator
	{
		public static string Generate ()
		{
			var id = Guid.NewGuid ();
			return id.ToString ();
		}
	}
}

