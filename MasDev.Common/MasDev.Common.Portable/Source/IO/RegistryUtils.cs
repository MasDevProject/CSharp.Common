using System.Threading.Tasks;
using MasDev.Common.Injection;


namespace MasDev.Common.IO
{
	public static class RegistryUtils
	{
		static readonly IRegistryProvider _registryProvider = Injector.Resolve<IRegistryProvider> ();


		public static void Put (string key, object value, string registryName)
		{
			var registry = _registryProvider.GetRegistry (registryName);
			registry.Prepare ();
			registry.Put (key, value);
			registry.Commit ();
		}



		public static T Read<T> (string key, T defaultVue, string registryName)
		{
			var registry = _registryProvider.GetRegistry (registryName);
			registry.Prepare ();
			return registry.Read<T> (key, defaultVue);
		}



		public static void Remove (string key, string registryName)
		{
			var registry = _registryProvider.GetRegistry (registryName);
			registry.Prepare ();
			registry.Remove (key);
			registry.Commit ();
		}


		public static void Remove (IRegistry registry, string key)
		{
			registry.Prepare ();
			registry.Remove (key);
			registry.Commit ();
		}



		public static async Task PutAsync (string key, object value, string registryName)
		{
			var registry = _registryProvider.GetRegistry (registryName);
			await registry.PrepareAsync ();
			registry.Put (key, value);
			await registry.CommitAsync ();
		}



		public static async Task<T> ReadAync<T> (string key, T defaultVue, string registryName)
		{
			var registry = _registryProvider.GetRegistry (registryName);
			await registry.PrepareAsync ();
			return registry.Read<T> (key, defaultVue);
		}



		public static async Task RemoveAsync (string key, string registryName)
		{
			var registry = _registryProvider.GetRegistry (registryName);
			await registry.PrepareAsync ();
			registry.Remove (key);
			await registry.CommitAsync ();
		}


		public static async void Clear (string registryName)
		{
			var registry = _registryProvider.GetRegistry (registryName);
			await registry.PrepareAsync ();
			registry.Clear ();
			await registry.CommitAsync ();
		}
	}
}

