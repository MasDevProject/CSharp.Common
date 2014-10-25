using MasDev.Common.Dependency;
using System.Threading.Tasks;
using System;

namespace MasDev.Common.Share.Tests
{
	public static class RepoManager
	{
		public static IPersonRepository PeopleRepository { get { return Injector.Resolve<IPersonRepository> (); } }

		public static IInterestRepository InterestsRepository { get { return Injector.Resolve<IInterestRepository> (); } }

		public static async Task InitializeAsync ()
		{
			try
			{
				await PeopleRepository.InitializeAsync ();
				await InterestsRepository.InitializeAsync ();
			} catch (Exception e)
			{
				var i = 0;
			}
		}

		public static void Dispose ()
		{
			PeopleRepository.Dispose ();
			InterestsRepository.Dispose ();
		}
	}
}
