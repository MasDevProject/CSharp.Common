using MasDev.Services.Modeling;


namespace MasDev.Services
{
	public interface IIdentityContext
	{
		Identity Identity { get; }

		int? Scope { get; }

		string Language { get; }
	}

	public class IdentityContext : IIdentityContext
	{
		readonly Identity _identity;
		readonly int? _scope;
		readonly string _language;

		public Identity Identity { get { return _identity; } }

		public int? Scope { get { return _scope; } }

		public string Language { get { return _language; } }


		public IdentityContext (Identity identity, string language = null, int? scope = null)
		{
			_identity = identity;
			_scope = scope;
			_language = language;
		}
	}
}