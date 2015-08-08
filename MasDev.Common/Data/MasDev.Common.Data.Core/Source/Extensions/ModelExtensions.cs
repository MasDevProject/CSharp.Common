using MasDev.Data;
using System.Security;


namespace MasDev.Extensions
{
	public static class ModelExtensions
	{
		public static void ThrowIfDeleted (this IUndeletableModel model)
		{
			if (model.IsDeleted)
				throw new SecurityException ("Model no longer enabled");
		}

		public static bool IsPersisted (this IModel model)
		{
			return model != null && model.Id > 0;
		}
	}
}

