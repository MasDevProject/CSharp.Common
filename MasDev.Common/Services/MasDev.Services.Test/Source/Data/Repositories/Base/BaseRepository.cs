using MasDev.Data;
using MasDev.Patterns.Injection;

namespace MasDev.Services.Test.Data
{
	public class BaseRepository<TModel> : NHibernateBaseRepository<TModel> where TModel : class, IModel, new()
	{
		public BaseRepository () : base (Injector.Resolve<IUnitOfWork> ())
		{
		}
	}
}

