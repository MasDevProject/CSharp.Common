namespace MasDev.Common
{
	public interface ICallingContext
	{
		IIdentity Identity { get; set; }

		int? Scope { get; set; }

		string Language { get; set; }
	}
}