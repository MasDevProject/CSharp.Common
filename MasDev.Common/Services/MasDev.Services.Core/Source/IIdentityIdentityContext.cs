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
		public Identity Identity { get; set; }

		public int? Scope { get ; set; }

		public string Language { get; set; }
	}
}