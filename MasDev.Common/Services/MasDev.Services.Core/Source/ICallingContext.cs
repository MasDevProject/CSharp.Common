using MasDev.Common;
using MasDev.Services.Modeling;


namespace MasDev.Services
{
	public sealed class CallingContext : ICallingContext
	{
		public Identity Identity { get; set; }

		public int? Scope { get ; set; }

		public string Language { get; set; }

		public string RequestPath { get; set; }

		public string RequestIp { get; set; }

		public string RequestHost { get; set; }
	}
}