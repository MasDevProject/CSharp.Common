using System.Net;


namespace MasDev.Rest
{
	public class ForbiddenException : BaseApiException
	{
		public ForbiddenException (string message) : base (HttpStatusCode.Forbidden, message)
		{
		}



		public ForbiddenException () : base (HttpStatusCode.Forbidden, null)
		{
		}



		public ForbiddenException (object obj) : base (HttpStatusCode.Forbidden, obj)
		{
		}
	}
}

