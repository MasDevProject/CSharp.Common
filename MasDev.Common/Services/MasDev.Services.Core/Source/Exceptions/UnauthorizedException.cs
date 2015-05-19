namespace MasDev.Services
{
	public class UnauthorizedException : ServiceException
	{
		public UnauthorizedException (object content = null) : base (content)
		{
		}
	}
}