

namespace MasDev.Common.Utils
{
	public static class FlagUtils
	{
		public static bool Has (int sourceFlag, int destFlag)
		{
			return (sourceFlag & destFlag) == destFlag;
		}



		public static bool HasExcactly (int sourceFlag, params int[] flags)
		{
			Assert.NotNullOrEmpty (flags);
			foreach (var flag in flags)
				if (sourceFlag == flag)
					return true; 
			return false;
		}
	}
}
