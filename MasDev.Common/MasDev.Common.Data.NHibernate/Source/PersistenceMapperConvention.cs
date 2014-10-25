using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using System.Collections.Generic;
using MasDev.Common.Modeling;
using System;
using System.Linq;
using MasDev.Common.Exceptions;


namespace MasDev.Common.Data.NHibernate
{
	public class PersistenceMapperConvention<TPersistenceMapper> : IPropertyConvention, IPropertyConventionAcceptance where TPersistenceMapper : PersistenceMapper, new()
	{
		readonly TPersistenceMapper _persistenceMapper = new TPersistenceMapper ();



		public void Accept (IAcceptanceCriteria<IPropertyInspector> criteria)
		{
			criteria
				.Expect (x => _persistenceMapper.IsRegistered (x.EntityType))
				.Expect (x =>
			{
				var modelMapper = _persistenceMapper.Get (x.EntityType);
				var typeOverloads = modelMapper.TypeProperties;
				var uniques = modelMapper.UniqueProperties;
				var uniqueKeys = modelMapper.UniqueKeyProperties;

				return 
				typeOverloads.Any (p => p.PropertyName == x.Name) ||
				uniques.Any (u => u.PropertyName == x.Name) ||
				uniqueKeys.Any (u => u.PropertyName == x.Name);
			});
		}



		public void Apply (IPropertyInstance instance)
		{
			var modelMapper = _persistenceMapper.Get (instance.EntityType);
			var hasApplied = false;
			var typeOverloads = modelMapper.TypeProperties;
			var uniques = modelMapper.UniqueProperties;
			var uniqueKeys = modelMapper.UniqueKeyProperties;

			var typeProperty = typeOverloads.Where (p => p.PropertyName == instance.Name).ToList ();
			if (typeProperty.Any ())
			{
				if (typeProperty.Count > 1)
					throw new Exception ("Multiple type mapping is unsupported");

				instance.CustomSqlType (GetSqlType (typeProperty.Single ().AlterType));
				hasApplied = true;
			}


			var uniqueProperty = uniques.Where (p => p.PropertyName == instance.Name).ToList ();
			if (uniqueProperty.Any ())
			{
				if (uniqueProperty.Count > 1)
					throw new Exception ("Multiple type mapping is unsupported");

				instance.Unique ();
				hasApplied = true;
			}


			var uniqueKeyProperty = uniqueKeys.Where (p => p.PropertyName == instance.Name).ToList ();
			if (uniqueKeyProperty.Any ())
			{
				if (uniqueKeyProperty.Count > 1)
					throw new Exception ("Multiple type mapping is unsupported");

				instance.UniqueKey (uniqueKeyProperty.Single ().KeyName);
				hasApplied = true;
			}

			if (!hasApplied)
				throw new ShouldNeverHappenException ("Convention apply failed");
		}



		static string GetSqlType (PersistenceType type)
		{
			switch (type)
			{
			case PersistenceType.Text:
				return "TEXT";
			default:
				throw new ArgumentException (type + " is not explicitly mapped to a SQL type");
			}
		}
	}
}

