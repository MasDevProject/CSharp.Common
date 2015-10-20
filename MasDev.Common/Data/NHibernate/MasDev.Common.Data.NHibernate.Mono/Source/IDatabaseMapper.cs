
using FluentNHibernate.Cfg;

namespace MasDev.Data.NHibernate
{
    public interface IDatabaseMapper
    {
        void ConfigureMappings(MappingConfiguration cfg);
    }
}
