using System;
using FluentNHibernate.Automapping;
using MasDev.Common.Modeling;
using FluentNHibernate;
using System.Linq;


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
			var result = base.ShouldMap (type) && typeof(IModel).IsAssignableFrom (type) && type.Namespace == _namespace;
			return result;
		}



		public override bool ShouldMap (Member member)
		{
			if (!base.ShouldMap (member))
				return false;

			var type = member.DeclaringType;
			var notVirtualProperties = type
				.GetProperties ()
				.Where (p => {
				var getMethod = p.GetMethod;
				if (getMethod != null && !getMethod.IsVirtual)
					return true;
				var setMethod = p.SetMethod;
				if (setMethod != null && !setMethod.IsVirtual)
					return true;
				return false;
			});

			if (notVirtualProperties.Any (p => p.Name == member.Name))
				return false;
				
			var metadata = _metadata.Get (member.DeclaringType);
			if (metadata == null)
				return true;
			return true;
		}
	}
}

