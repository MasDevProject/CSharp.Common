using MasDev.Common.Modeling;
using System.Security;


namespace MasDev.Common.Extensions
{
	public static class ModelExtensions
	{
		public static void ThrowIfNotEnabled (this IUndeletableModel model)
		{
			if (!model.IsEnabled)
				throw new SecurityException ("Model no longer enabled");
		}
	}
}

