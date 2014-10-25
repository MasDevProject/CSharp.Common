using System;
using System.Net;


namespace MasDev.Common.Rest
{
	public class BaseApiException : Exception
	{
		public ApiExceptionContent Content { get; private set; }



		public BaseApiException (HttpStatusCode statusCode, object content, int? additionalInformation = null) : base (statusCode.ToString ())
		{
			Content = new ApiExceptionContent (statusCode, additionalInformation, content);
		}
	}





	public sealed class ApiExceptionContent
	{
		public HttpStatusCode StatusCode { get; private set; }



		public int? AdditionalInformation { get; private set; }



		public object Content { get; private set; }



		public ApiExceptionContent (HttpStatusCode statusCode, int? additionalInformation, object content)
		{
			StatusCode = statusCode;
			AdditionalInformation = additionalInformation;
			Content = content;
		}
	}
}

