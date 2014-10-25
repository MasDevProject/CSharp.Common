using System.Net;


namespace MasDev.Common.Rest
{
	public class BadRequestException : BaseApiException
	{
		public BadRequestException (object content, int additionalInfo) : base (HttpStatusCode.BadRequest, content, additionalInfo)
		{
		}



		public BadRequestException (int additionalInfo) : base (HttpStatusCode.BadRequest, null, additionalInfo)
		{
		}
	}
}

