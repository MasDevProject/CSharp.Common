using AutoMapper;
using System.Collections.Generic;
using System;


namespace MasDev.AutoMapper
{
	public class AutoMapperConfiguration
	{
		readonly Dictionary<Type, Type> _map = new Dictionary<Type, Type> ();



		public void AddMapping<TSource, TDestination> ()
		{
			Mapper.CreateMap<TSource, TDestination> ().ReverseMap ();
		}



		public void AddMapping<TSource, TDestination> (ITypeConverter<TSource, TDestination> converter)
		{
			Mapper.CreateMap<TSource, TDestination> ().ConvertUsing (converter);
		}



		public Type GetMapping (Type source)
		{
			return _map [source];
		}



		public static AutoMapperConfiguration Current { get; set; }
	}
}

