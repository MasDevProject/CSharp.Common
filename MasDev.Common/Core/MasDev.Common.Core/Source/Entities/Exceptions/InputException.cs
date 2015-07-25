namespace MasDev.Common
{
	public class BadRequestException : ServiceException
	{
		public BadRequestException ()
		{
			
		}

		public BadRequestException (int additionalInformation, object content = null) : base (additionalInformation, content)
		{
		}
	}
}