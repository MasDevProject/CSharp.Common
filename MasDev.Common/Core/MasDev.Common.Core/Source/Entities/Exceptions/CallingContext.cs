using MasDev.Common;
using MasDev.Services.Modeling;
using MasDev.Patterns.Injection;


namespace MasDev.Common
{
	public sealed class CallingContext : ICallingContext
	{
		public Identity Identity { get; set; }

		public int? Scope { get ; set; }

		public string Language { get; set; }

		public string RequestPath { get; set; }

		public string RequestIp { get; set; }

		public string RequestHost { get; set; }

		public MultiValueDictionary<string, string> RequestHeaders { get; set; }

		public MultiValueDictionary<string, string> ResponseHeaders { get; set; }

		public static ICallingContext Current { get { return Injector.Resolve<ICallingContext> (); } }
	}
}