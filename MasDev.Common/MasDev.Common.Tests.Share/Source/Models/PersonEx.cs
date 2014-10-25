using System;
using MasDev.Common.Share.Tests.Models;
using System.Linq;

namespace MasDev.Common.Share.Tests
{
	public static class PersonEx
	{
		public static bool HasInterests (this Person p)
		{
			return p.Interests != null && p.Interests.Any ();
		}
	}
}

