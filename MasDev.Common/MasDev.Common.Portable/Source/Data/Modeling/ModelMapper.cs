using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using MasDev.Common.Modeling;
using MasDev.Common.Injection;
using System.Linq;


namespace MasDev.Common.Modeling
{
	public class ModelMapper
	{
		protected readonly IPropertyNameResolver _resolver;
		readonly ISet<PropertyMapper> _mappers;
		readonly Type _modelType;



		protected ModelMapper (Type modelType)
		{
			_resolver = Injector.Resolve<IPropertyNameResolver> ();
			_mappers = new HashSet<PropertyMapper> ();
			_modelType = modelType;
		}



		public Type ModelType { get { return _modelType; } }



		public IEnumerable<UniqueProperty> UniqueProperties { get { return GetUniqueProperties (); } }



		public IEnumerable<UniqueKeyProperty> UniqueKeyProperties { get { return GetUniqueKeyProperties (); } }



		public IEnumerable<TypeProperty> TypeProperties { get { return GetTypeProperties (); } }



		IEnumerable<UniqueProperty> GetUniqueProperties ()
		{
			foreach (var mapper in _mappers)
				foreach (var unique in mapper.UniqueProperies)
					yield return unique;
		}



		IEnumerable<UniqueKeyProperty> GetUniqueKeyProperties ()
		{
			foreach (var mapper in _mappers)
				foreach (var unique in mapper.UniqueKeyProperies)
					yield return unique;
		}



		IEnumerable<TypeProperty> GetTypeProperties ()
		{
			foreach (var mapper in _mappers)
				foreach (var unique in mapper.TypeProperties)
					yield return unique;
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
			var propertyName = _resolver.Resolve (property);
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



		public PropertyMapper (string propertyName)
		{
			_propertyName = propertyName;
			_typeChanges = new HashSet<TypeProperty> ();
			_uniques = new HashSet<UniqueProperty> ();
			_uniqueKeys = new HashSet<UniqueKeyProperty> ();
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



		public IEnumerable<TypeProperty> TypeProperties { get { return _typeChanges.AsEnumerable (); } }



		public IEnumerable<UniqueKeyProperty> UniqueKeyProperies { get { return _uniqueKeys.AsEnumerable (); } }



		public IEnumerable<UniqueProperty> UniqueProperies { get { return _uniques.AsEnumerable (); } }
	}






	public enum PersistenceType
	{
		Text,
	}





	public interface IPropertyNameResolver
	{
		string Resolve<TModel, TKey> (Expression<Func<TModel, TKey>> propertyExpr) where TModel : IModel;
	}





	public class TypeProperty
	{
		public PersistenceType AlterType { get; private set; }



		public string PropertyName { get; private set; }



		public TypeProperty (PersistenceType alterType, string propertyName)
		{
			AlterType = alterType;
			PropertyName = propertyName;
		}
	}





	public class UniqueProperty
	{
		public string PropertyName { get; set; }



		public UniqueProperty (string propertyName)
		{
			PropertyName = propertyName;
		}
	}





	public class UniqueKeyProperty
	{
		public string KeyName { get; private set; }



		public string PropertyName { get; private set; }



		public UniqueKeyProperty (string keyName, string propertyName)
		{
			KeyName = keyName;
			PropertyName = propertyName;
		}
	}


}

