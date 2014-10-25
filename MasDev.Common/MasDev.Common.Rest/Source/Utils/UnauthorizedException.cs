using System.Net;


namespace MasDev.Common.Rest
{
	public class UnauthorizedException : BaseApiException
	{
		public UnauthorizedException (string message) : base (HttpStatusCode.Unauthorized, message)
		{
		}
	}
}

