namespace MasDev.Services
{
	public class SecurityException : ServiceException
	{
		public SecurityException (object content = null) : base (content)
		{
		}
	}
}

