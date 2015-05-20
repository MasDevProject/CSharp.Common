namespace MasDev.Common
{
	public interface IIdentityContext
	{
		IIdentity Identity { get; }

		int? Scope { get; }

		string Language { get; }
	}
}