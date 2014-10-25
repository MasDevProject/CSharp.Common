namespace MasDev.Common.Utils
{
	public static class MathUtils
	{
		public static decimal AddPercentage (decimal value, decimal percentage)
		{
			return value + (value * percentage);
		}



		public static decimal AddPercentage (decimal value, int percentage)
		{
			return value + ((value * (decimal)percentage) / 100m);
		}



		public static double AddPercentage (double value, double percentage)
		{
			return value + (value * percentage);
		}



		public static double AddPercentage (double value, int percentage)
		{
			return value + ((value * (double)percentage) / 100d);
		}
	}
}

