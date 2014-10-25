

namespace MasDev.Common.Modeling
{
	public static class Model
	{
		public static TModel FromId<TModel> (int id) where TModel : IModel, new()
		{
			return new TModel{ Id = id };
		}
	}
}

