

namespace MasDev.Common.Patterns
{
	public interface ISingleton<T> where T : class, new()
	{
		T Instance { get;}
	}

	public class LazySingleton<T> : ISingleton<T> where T : class, new()
	{
		const string _lock = "6";

		static T _instance;



		public T Instance
		{
			get {
				lock (_lock)
				{
					if (_instance == null)
						_instance = new T ();
				}
				return _instance;
			}
		}
	}

	public class Singleton<T> : ISingleton<T> where T : class, new()
	{
		const string _lock = "6";

		static T _instance;

		public Singleton()
		{
			lock (_lock)
			{
				if (_instance == null)
					_instance = new T ();
			}
		}

		public T Instance
		{
			get {
				return _instance;
			}
		}
	}
}

