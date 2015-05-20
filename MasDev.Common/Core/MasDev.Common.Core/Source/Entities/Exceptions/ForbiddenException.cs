namespace MasDev.Common
{
	public class SecurityException : ServiceException
	{
		public SecurityException (object content = null) : base (content)
		{
		}
	}
}

