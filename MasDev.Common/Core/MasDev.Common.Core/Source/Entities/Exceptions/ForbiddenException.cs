namespace MasDev.Common
{
	public class ForbiddenException : ServiceException
	{
		public ForbiddenException (object content = null) : base (content)
		{
		}
	}
}

