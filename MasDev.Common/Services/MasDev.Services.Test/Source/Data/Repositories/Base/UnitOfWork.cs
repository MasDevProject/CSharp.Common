using MasDev.Data;
using MasDev.Patterns.Injection;
using MasDev.Data.NHibernate;

namespace MasDev.Services.Test.Data
{
	public class UnitOfWork : NHibernateUnitOfWork
	{
		public UnitOfWork () : base (Injector.Resolve<ISessionFactoryProvider> ().Factory)
		{
			
		}
	}
}

