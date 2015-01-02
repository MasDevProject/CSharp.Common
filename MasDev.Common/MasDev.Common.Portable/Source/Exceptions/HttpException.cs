using System;
using System.Net;


namespace MasDev.Exceptions
{
	public sealed class HttpException : Exception
	{
		public HttpStatusCode StatusCode { get; private set; }



		public HttpException (HttpStatusCode code) : base (code.ToString ())
		{
			StatusCode = code;
		}
	}
}

