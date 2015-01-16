using System;
using MasDev.Reflection;
using Newtonsoft.Json;


namespace MasDev.Utils
{
	public static class SerializationUtils
	{
		public static string Serialize<T> (T arg) where T : class
		{
			if (arg == null)
				return null;

			var type = arg.GetType ();
			if (Types.IsNativeType (type))
			{
				var serialized = arg.ToString ();
				return Types.IsRealNumber (type) ? serialized.Replace (',', '.') : serialized;
			}

			return JsonConvert.SerializeObject (arg);
		}



		public static string Serialize (object arg)
		{
			if (arg == null)
				return null;

			var type = arg.GetType ();
			if (Types.IsNativeType (type))
			{
				var serialized = arg.ToString ();
				return Types.IsRealNumber (type) ? serialized.Replace (',', '.') : serialized;
			}

			return JsonConvert.SerializeObject (arg);
		}
	}
}

