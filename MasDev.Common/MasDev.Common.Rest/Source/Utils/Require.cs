using System;


namespace MasDev.Rest
{
	public static class Require
	{
		public static void NotNull (params object[] objs)
		{
			foreach (var obj in objs)
				if (obj == null)
					throw new BadRequestException (-1);
		}
	}
}

