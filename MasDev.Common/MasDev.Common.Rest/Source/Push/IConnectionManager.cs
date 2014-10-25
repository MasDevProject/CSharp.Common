using System.Collections.Generic;
using MasDev.Common.Rest.Push;
using System.Threading.Tasks;


namespace MasDev.Common.Rest.Push
{
	public interface IReadonlyConnectionManager
	{
		Task<ICollection<string>> ConnectionsForModel (int modelId);
	}





	public interface IConnectionManager : IReadonlyConnectionManager
	{
		Task AddConnection (int modelId, string connectionId);



		Task RemoveConnection (int modelId, string connectionId);



		Task AddConnectionIfNotStored (int modelId, string connectionId);
	}





	public static class ConnectionManagerExtensions
	{
		public static IReadonlyConnectionManager AsReadOnly (this IConnectionManager manager)
		{
			return new ReadonlyConnectionManager (manager);
		}
	}





	class ReadonlyConnectionManager : IReadonlyConnectionManager
	{
		readonly IReadonlyConnectionManager _wrapped;



		public ReadonlyConnectionManager (IReadonlyConnectionManager manager)
		{
			_wrapped = manager;
		}



		public async Task<ICollection<string>> ConnectionsForModel (int modelId)
		{
			return await _wrapped.ConnectionsForModel (modelId);
		}
	}
}

