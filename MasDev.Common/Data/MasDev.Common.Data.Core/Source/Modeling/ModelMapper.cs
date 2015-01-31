using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using MasDev.Data;
using MasDev.Patterns.Injection;
using System.Linq;


namespace MasDev.Data
{
	public class ModelMapper
	{
		protected readonly IPropertyNameResolver Resolver;
		readonly ISet<PropertyMapper> _mappers;
		readonly Type _modelType;



		protected ModelMapper (Type modelType)
		{
			Resolver = Injector.Resolve<IPropertyNameResolver> ();
			_mappers = new HashSet<PropertyMapper> ();
			_modelType = modelType;
		}

		public Type ModelType { get { return _modelType; } }

		public IEnumerable<UniqueProperty> UniqueProperties {
			get { 
				foreach (var mapper in _mappers)
					foreach (var unique in mapper.UniqueProperies)
						yield return unique;
			}
		}

		public IEnumerable<UniqueKeyProperty> UniqueKeyProperties {
			get { 
				foreach (var mapper in _mappers)
					foreach (var unique in mapper.UniqueKeyProperies)
						yield return unique;
			}
		}

		public IEnumerable<TypeProperty> TypeProperties {
			get { 
				foreach (var mapper in _mappers)
					foreach (var unique in mapper.TypeProperties)
						yield return unique;
			}
		}

		public IEnumerable<NotLazyProperty> NotLazyProperties {
			get {
				foreach (var mapper in _mappers)
					foreach (var notLazy in mapper.NotLazyProperties)
						yield return notLazy;
			}
		}

		internal void AddPropertyMapper (PropertyMapper mapper)
		{
			_mappers.Add (mapper);
		}
	}

	public abstract class ModelMapper<TModel> : ModelMapper where TModel : IModel
	{
		protected ModelMapper () : base (typeof(TModel))
		{
		}

		public PropertyMapper Map<TKey> (Expression<Func<TModel, TKey>> property)
		{
			var propertyName = Resolver.Resolve (property);
			var mapper = new PropertyMapper (propertyName);
			AddPropertyMapper (mapper);
			return mapper;
		}

		public abstract void Map ();
	}

	public class PropertyMapper
	{
		readonly string _propertyName;
		readonly ISet<TypeProperty> _typeChanges;
		readonly ISet<UniqueKeyProperty> _uniqueKeys;
		readonly ISet<UniqueProperty> _uniques;
		readonly ISet<NotLazyProperty> _notLazies;



		public PropertyMapper (string propertyName)
		{
			_propertyName = propertyName;
			_typeChanges = new HashSet<TypeProperty> ();
			_uniques = new HashSet<UniqueProperty> ();
			_uniqueKeys = new HashSet<UniqueKeyProperty> ();
			_notLazies = new HashSet<NotLazyProperty> ();
		}

		public PropertyMapper SetPersistenceType (PersistenceType type)
		{
			_typeChanges.Add (new TypeProperty (type, _propertyName));
			return this;
		}

		public PropertyMapper Unique ()
		{ 
			_uniques.Add (new UniqueProperty (_propertyName));
			return this;
		}

		public PropertyMapper UniqueKey (string keyName)
		{
			_uniqueKeys.Add (new UniqueKeyProperty (keyName, _propertyName));
			return this;
		}

		public PropertyMapper NotLazy ()
		{
			_notLazies.Add (new NotLazyProperty (_propertyName));
			return this;
		}

		public IEnumerable<TypeProperty> TypeProperties { get { return _typeChanges.AsEnumerable (); } }

		public IEnumerable<UniqueKeyProperty> UniqueKeyProperies { get { return _uniqueKeys.AsEnumerable (); } }

		public IEnumerable<UniqueProperty> UniqueProperies { get { return _uniques.AsEnumerable (); } }

		public IEnumerable<NotLazyProperty> NotLazyProperties { get { return _notLazies.AsEnumerable (); } }
	}

	public enum PersistenceType
	{
		Text,
	}

	public interface IPropertyNameResolver
	{
		string Resolve<TModel, TKey> (Expression<Func<TModel, TKey>> propertyExpr) where TModel : IModel;
	}

	public interface IMappedProperty
	{
		string PropertyName { get; }
	}

	public class TypeProperty : IMappedProperty
	{
		readonly string _propertyName;

		public PersistenceType AlterType { get; private set; }

		public string PropertyName { get { return _propertyName; } }

		public TypeProperty (PersistenceType alterType, string propertyName)
		{
			AlterType = alterType;
			_propertyName = propertyName;
		}
	}

	public class UniqueProperty : IMappedProperty
	{
		readonly string _propertyName;

		public UniqueProperty (string propertyName)
		{
			_propertyName = propertyName;
		}

		public string PropertyName { get { return _propertyName; } }
	}

	public class UniqueKeyProperty : IMappedProperty
	{
		readonly string _propertyName;

		public string KeyName { get; private set; }

		public string PropertyName { get { return _propertyName; } }

		public UniqueKeyProperty (string keyName, string propertyName)
		{
			KeyName = keyName;
			_propertyName = propertyName;
		}
	}

	public class NotLazyProperty : IMappedProperty
	{
		readonly string _propertyName;

		public NotLazyProperty (string propertyName)
		{
			_propertyName = propertyName;
		}

		public string PropertyName { get { return _propertyName; } }
	}


}

