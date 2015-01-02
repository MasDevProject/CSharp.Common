using System.Collections.Generic;
using MasDev.IO;
using System.Threading.Tasks;


namespace MasDev.Extensions
{
	public static class RegistryExtensions
	{
		public static void AddStringToSet (this IRegistry registry, string key, string s)
		{
			var set = registry.Read<HashSet<string>> (key, new HashSet<string> ());
			set.Add (s);
			registry.Put (key, set);
		}



		public static void RemoveAll (this IRegistry registry)
		{
			foreach (var key in registry.Keys)
				registry.Remove (key);
		}


        public static void PutCommitting(this IRegistry registry, string key, object value)
        {
            registry.Put(key, value);
            registry.Commit();
        }


        public static async Task PutCommittingAsync(this IRegistry registry, string key, object value)
        {
            registry.Put(key, value);
            await registry.CommitAsync();
        }
	}
}

