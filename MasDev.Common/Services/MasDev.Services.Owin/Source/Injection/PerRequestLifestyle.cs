using SimpleInjector.Extensions.ExecutionContextScoping;


namespace MasDev.Services
{
	public static class PerRequestLifestyle
	{
		public static readonly ExecutionContextScopeLifestyle Instance = new ExecutionContextScopeLifestyle ();
	}
}

