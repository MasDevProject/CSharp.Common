using System.Linq;
using MasDev.Common.Modeling;


namespace MasDev.Common.Data
{
	public interface IModelQueryFactory
	{
		IQueryable<TModel> QueryForModel<TModel> () where TModel : IModel;
	}
}

