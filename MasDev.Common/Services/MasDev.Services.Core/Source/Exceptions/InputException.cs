namespace MasDev.Services
{
	public class InputException : ServiceException
	{
		public InputException ()
		{
			
		}

		public InputException (int additionalInformation, object content = null) : base (additionalInformation, content)
		{
		}
	}
}