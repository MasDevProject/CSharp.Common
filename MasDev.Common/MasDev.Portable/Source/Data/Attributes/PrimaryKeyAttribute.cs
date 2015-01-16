using System;

namespace MasDev.Common
{
	#define SqliteNet 1

	#ifdef SqliteNet
	public class PrimaryKeyAttribute
	{
		public PrimaryKeyAttribute ()
		{
		}
	}
	#endif
}

