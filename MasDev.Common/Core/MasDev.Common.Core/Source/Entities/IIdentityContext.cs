namespace MasDev.Common
{
	public interface IIdentityContext
	{
		IIdentity Identity { get; set; }

		int? Scope { get; set; }

		string Language { get; set; }
	}
}