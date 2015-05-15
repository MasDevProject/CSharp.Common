using MasDev.Data;
using MasDev.Patterns.Injection;
using System;

namespace MasDev.Services.Test.Data
{
	public class BaseRepository<TModel> : NHibernateBaseRepository<TModel> where TModel : class, IModel, new()
	{
		public BaseRepository () : base (Injector.Resolve<IUnitOfWork> ())
		{
			Console.WriteLine ("{0} constructed", typeof(TModel).Name);
		}

		public override void Dispose ()
		{
			Console.WriteLine ("{0} disposed", typeof(TModel).Name);
			base.Dispose ();
		}
	}
}

