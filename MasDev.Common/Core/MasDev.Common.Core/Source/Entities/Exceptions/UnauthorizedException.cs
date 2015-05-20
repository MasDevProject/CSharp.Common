namespace MasDev.Common
{
	public class UnauthorizedException : ServiceException
	{
		public UnauthorizedException (object content = null) : base (content)
		{
		}
	}
}