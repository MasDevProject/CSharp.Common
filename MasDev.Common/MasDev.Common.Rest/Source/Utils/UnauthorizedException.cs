using System.Net;


namespace MasDev.Rest
{
	public class UnauthorizedException : BaseApiException
	{
		public UnauthorizedException (string message) : base (HttpStatusCode.Unauthorized, message)
		{
		}
	}
}

