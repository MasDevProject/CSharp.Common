using System;

namespace MasDev.Services
{
	public class NotFoundException : ServiceException
	{
		public NotFoundException (object content = null) : base (content)
		{
		}
	}
}

