using System.Linq;
using MasDev.Data;


namespace MasDev.Data
{
	public interface IModelQueryFactory
	{
		IQueryable<TModel> QueryForModel<TModel> () where TModel : IUndeletableModel;

		IQueryable<TModel> UnfilteredQueryForModel<TModel> () where TModel : IModel;
	}
}

