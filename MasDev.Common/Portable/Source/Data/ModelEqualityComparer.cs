using MasDev.Data;
using System.Collections.Generic;
using MasDev.Utils;


namespace MasDev.Data
{
	public class ModelEqualityComparer<TModel> : IEqualityComparer<TModel> where TModel : class, IModel, new()
	{
		public bool Equals (TModel x, TModel y)
		{
			if (Check.BothNull (x, y))
				return true;

			if (!Check.BothNotNull (x, y))
				return false;

			return x.Id == y.Id;
		}



		public int GetHashCode (TModel obj)
		{
			return obj.GetHashCode ();
		}
	}
}

