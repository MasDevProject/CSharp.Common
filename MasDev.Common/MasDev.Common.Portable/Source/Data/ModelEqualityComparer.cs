using MasDev.Common.Modeling;
using System.Collections.Generic;


namespace MasDev.Common.Data
{
	public class ModelEqualityComparer<TModel> : IEqualityComparer<TModel> where TModel : class, IModel, new()
	{
		public bool Equals (TModel x, TModel y)
		{
			if (x == null && y == null)
				return true;

			if (x == null && y != null)
				return false;

			if (y == null && x != null)
				return false;

			return x.Id == y.Id;
		}



		public int GetHashCode (TModel obj)
		{
			return obj.GetHashCode ();
		}
	}
}

