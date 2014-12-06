using System;


namespace MasDev.Common.Exceptions
{
	public class ObjectException : Exception
	{
		public virtual dynamic Object { get; private set; }

		public ObjectException (dynamic obj)
		{
			Object = obj;
		}
	}

	public class ObjectException<T> : ObjectException
	{
		public new T Object { get; private set; }



		public ObjectException (T obj) : base (obj)
		{
			Object = obj;
		}
	}
}

