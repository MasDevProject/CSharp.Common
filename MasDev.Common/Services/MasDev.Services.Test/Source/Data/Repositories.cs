using MasDev.Services.Test.Data;
using MasDev.Patterns.Injection;


namespace MasDev.Services.Test
{
	public static class Repositories
	{
		public static IUsersRepository Users { get { return Injector.Resolve<IUsersRepository> (); } }
	}
}

