using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using System.Collections.Generic;
using MasDev.Data;
using System;
using System.Linq;
using MasDev.Exceptions;


namespace MasDev.Data.NHibernate
{
	public class PersistenceMapperConvention<TPersistenceMapper> : IPropertyConvention, IPropertyConventionAcceptance where TPersistenceMapper : PersistenceMapper, new()
	{
		readonly TPersistenceMapper _persistenceMapper = new TPersistenceMapper ();

		public void Accept (IAcceptanceCriteria<IPropertyInspector> criteria)
		{
			criteria
				.Expect (x => _persistenceMapper.IsRegistered (x.EntityType))
				.Expect (x => {
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

			ApplyTypeOverloads (instance, modelMapper.TypeProperties, ref hasApplied);
			ApplyNotLazies (instance, modelMapper.NotLazyProperties, ref hasApplied);
			ApplyUniques (instance, modelMapper.UniqueProperties, ref hasApplied);
			ApplyUniqueKeys (instance, modelMapper.UniqueKeyProperties, ref hasApplied);

			if (!hasApplied)
				throw new ShouldNeverHappenException ("Convention apply failed");
		}

		static void ApplyTypeOverloads (IPropertyInstance instance, IEnumerable<TypeProperty> typeOverloads, ref bool hasApplied)
		{
			var typeProperties = ByInstanceName (typeOverloads, instance);
			if (!typeProperties.Any ())
				return;

			if (typeProperties.Count > 1)
				throw new Exception ("Multiple type mapping is unsupported");

			var typeProperty = typeProperties.Single ();

			if (typeProperty.AlterType == PersistenceType.Text) {
				//instance.CustomType<AnsiStringFixedLengthSqlType>();
				//instance.Length(5000);
				instance.CustomSqlType ("TEXT");
			}
			hasApplied = true;
		}

		static void ApplyNotLazies (IPropertyInstance instance, IEnumerable<NotLazyProperty> notLazies, ref bool hasApplied)
		{
			var notLazyProperties = ByInstanceName (notLazies, instance);
			if (!notLazyProperties.Any ())
				return;

			if (notLazyProperties.Count > 1)
				throw new Exception ("Multiple not lazy mapping is unsupported");

			instance.Not.LazyLoad ();
			hasApplied = true;
		}

		static void ApplyUniques (IPropertyInstance instance, IEnumerable<UniqueProperty> uniques, ref bool hasApplied)
		{
			var uniqueProperty = ByInstanceName (uniques, instance);
			if (!uniqueProperty.Any ())
				return;
			if (uniqueProperty.Count > 1)
				throw new Exception ("Multiple unique mapping is unsupported");

			instance.Unique ();
			hasApplied = true;
		}

		static void ApplyUniqueKeys (IPropertyInstance instance, IEnumerable<UniqueKeyProperty> uniqueKeys, ref bool hasApplied)
		{
			var uniqueKeyProperty = ByInstanceName (uniqueKeys, instance);
			if (!uniqueKeyProperty.Any ())
				return;
			if (uniqueKeyProperty.Count > 1)
				throw new Exception ("Multiple unique key mapping is unsupported");

			instance.UniqueKey (uniqueKeyProperty.Single ().KeyName);
			hasApplied = true;
		}

		static ICollection<TMappedProperty> ByInstanceName<TMappedProperty> (IEnumerable<TMappedProperty> properties, IPropertyInstance instance) where TMappedProperty : IMappedProperty
		{
			return properties.Where (p => p.PropertyName == instance.Name).ToList ();
		}

		static string GetSqlType (PersistenceType type)
		{
			switch (type) {
			case PersistenceType.Text:
				return "TEXT";
			default:
				throw new ArgumentException (type + " is not explicitly mapped to a SQL type");
			}
		}
	}
}

