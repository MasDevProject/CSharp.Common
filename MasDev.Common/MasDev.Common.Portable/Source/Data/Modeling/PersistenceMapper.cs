using System;
using System.Collections.Generic;
using MasDev.Common.Modeling;


namespace MasDev.Common.Data
{
	public class PersistenceMapper
	{
		readonly Dictionary<Type, ModelMapper> _dict = new Dictionary<Type, ModelMapper> ();



		public void Register<TModel, TModelMapper> () where TModel : IModel where TModelMapper : ModelMapper<TModel>, new()
		{
			var modelMapper = new TModelMapper ();
			modelMapper.Map ();
			_dict.Add (typeof(TModel), modelMapper);
		}



		public ModelMapper Get<TModel> () where TModel : IModel
		{
			if (!_dict.ContainsKey (typeof(TModel)))
				return null;

			return _dict [typeof(TModel)];
		}



		public bool IsRegistered (Type t)
		{
			return _dict.ContainsKey (t);
		}



		public bool IsRegistered<T> ()
		{
			return IsRegistered (typeof(T));
		}



		public ModelMapper Get (Type t)
		{
			if (!_dict.ContainsKey (t))
				return null;

			return _dict [t];
		}
	}
}

