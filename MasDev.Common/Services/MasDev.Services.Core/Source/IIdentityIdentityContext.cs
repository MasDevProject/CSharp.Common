using MasDev.Services.Modeling;


namespace MasDev.Services
{
	public interface IIdentityContext
	{
		Identity Identity { get; }

		int? Scope { get; }
	}

	public class IdentityContext : IIdentityContext
	{
		readonly Identity _identity;
		readonly int? _scope;

		public Identity Identity { get { return _identity; } }

		public int? Scope { get { return _scope; } }

		public IdentityContext (Identity identity, int? scope = null)
		{
			_identity = identity;
			_scope = scope;
		}
	}
}