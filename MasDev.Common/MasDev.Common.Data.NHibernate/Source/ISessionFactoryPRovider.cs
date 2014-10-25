

using NHibernate;

namespace MasDev.Common.Data.NHibernate
{
    public interface ISessionFactoryProvider
    {
        ISessionFactory Factory { get; }
    }
}
