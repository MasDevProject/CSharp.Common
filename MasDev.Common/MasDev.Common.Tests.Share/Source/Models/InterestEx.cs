using MasDev.Common.Share.Tests.Models;
using System.Collections.Generic;

namespace MasDev.Common.Share.Tests
{
	public static class InterestEx
	{
		public static void AddInterestedPerson (this Interest i, Person p)
		{
			if (i.PersonInterestedIn == null)
				i.PersonInterestedIn = new List<Person> ();

			i.PersonInterestedIn.Add (p);
		}
	}
}

