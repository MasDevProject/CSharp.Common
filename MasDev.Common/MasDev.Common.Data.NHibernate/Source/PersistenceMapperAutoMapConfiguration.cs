using System;
using FluentNHibernate.Automapping;
using MasDev.Common.Modeling;
using FluentNHibernate;


namespace MasDev.Common.Data.NHibernate
{
	public class PersistenceMapperAutoMapConfiguration<TPersistenceMapper> : DefaultAutomappingConfiguration where TPersistenceMapper : PersistenceMapper, new()
	{
		readonly string _namespace;
		readonly TPersistenceMapper _metadata;



		public PersistenceMapperAutoMapConfiguration (string ns)
		{
			_namespace = ns;
			_metadata = new TPersistenceMapper ();
		}



		public override bool ShouldMap (Type type)
		{
			return base.ShouldMap (type) && typeof(IModel).IsAssignableFrom (type) && type.Namespace == _namespace;
		}



		public override bool ShouldMap (Member member)
		{
			if (!base.ShouldMap (member))
				return false;

			var metadata = _metadata.Get (member.DeclaringType);
			if (metadata == null)
				return true;
			return true;
		}
	}
}

