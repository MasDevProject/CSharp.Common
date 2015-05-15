

using NHibernate;

namespace MasDev.Data.NHibernate
{
	public interface ISessionFactoryProvider
	{
		ISessionFactory Factory { get; }

		void Connect ();
	}
}
