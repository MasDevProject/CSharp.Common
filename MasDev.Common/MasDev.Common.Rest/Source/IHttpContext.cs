using System.Collections.Generic;
using System;


namespace MasDev.Rest
{
	public interface IHttpContext : IDisposable
	{
		string RemoteIpAddress { get; }



		Dictionary<string, IEnumerable<string>> RequestHeaders { get; }



		Dictionary<string, IEnumerable<string>> ResponseHeaders { get; set; }
	}
}

