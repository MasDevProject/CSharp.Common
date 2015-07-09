using MasDev.Common;


namespace MasDev.Services
{
	public sealed class CallingContext : ICallingContext
	{
		public IIdentity Identity { get; set; }

		public int? Scope { get ; set; }

		public string Language { get; set; }

		public string RequestPath { get; set; }
	}
}