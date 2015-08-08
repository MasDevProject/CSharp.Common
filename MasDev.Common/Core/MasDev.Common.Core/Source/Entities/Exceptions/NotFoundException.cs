
namespace MasDev.Common
{
	public class NotFoundException : ServiceException
	{
		public NotFoundException (object content = null) : base (content)
		{
		}
	}
}

