namespace MasDev.Common.Saltarelle
{
	public static class Require
	{
		public static T Load<T> (string moduleName)
		{
			return NodeJS.Require.Load (moduleName);
		}
	}
}

