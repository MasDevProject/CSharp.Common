using System.Threading.Tasks;
using System.Collections.Generic;


namespace MasDev.Common.IO
{
	public interface IRegistryProvider
	{
		IRegistry GetRegistry (string registryName);
	}


	public interface IRegistry
	{
		void Put (string key, object value);



		T Read<T> (string key, T defaultValue);



		void Remove (string key);



		void Prepare ();



		Task PrepareAsync ();



		void Commit ();



		Task CommitAsync ();



		void Rollback ();


		void Clear ();


		RegistryConfiguration Configuration { set; }



		IEnumerable<string> Keys { get; }
	}





	public class RegistryConfiguration
	{
		public bool Encrypt { get; set; }



		public string EncryptionKey { get; set; }
	}
}

