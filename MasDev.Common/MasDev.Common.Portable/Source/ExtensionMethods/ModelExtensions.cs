using MasDev.Common.Modeling;
using System.Security;


namespace MasDev.Common.Extensions
{
	public static class ModelExtensions
	{
		public static void ThrowIfDeleted (this IUndeletableModel model)
		{
			if (model.IsDeleted)
				throw new SecurityException ("Model no longer enabled");
		}
	}
}

