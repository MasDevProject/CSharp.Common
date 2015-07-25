using MasDev.Services.Modeling;

namespace MasDev.Common
{
	public interface ICallingContext
	{
		Identity Identity { get; set; }

		int? Scope { get; set; }

		string Language { get; set; }

		string RequestPath { get; set; }

		string RequestIp { get; set; }

		string RequestHost { get; set; }
	}
}