using System;


namespace MasDev.Common.Exceptions
{
	public class ObjectException<T> : Exception
	{
		public T Object { get; private set; }



		public ObjectException (T obj)
		{
			Object = obj;
		}
	}
}

