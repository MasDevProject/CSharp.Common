using System.Collections.Generic;
using MasDev.Rest.Push;
using System.Threading.Tasks;
using MasDev.Data;


namespace MasDev.Rest.Push
{
	public interface IReadonlyConnectionManager<out TModel> where TModel : class, IModel, new()
	{
		Task<ICollection<string>> ConnectionsForModel (int modelId);
	}

	public interface IConnectionManager<out TModel> : IReadonlyConnectionManager<TModel> where TModel:class, IModel, new()
	{
		Task AddConnection (int modelId, string connectionId);



		Task RemoveConnection (int modelId, string connectionId);



		Task RemoveConnection (string connectionId);



		Task AddConnectionIfNotStored (int modelId, string connectionId);
	}

	public interface IConnectionManagerFactory
	{
		IConnectionManager<TModel> Create<TModel> (IRepositories repositories) where TModel : class, IModel, new();
	}


	public static class ConnectionManagerExtensions
	{
		public static IReadonlyConnectionManager<TModel> AsReadOnly<TModel> (this IConnectionManager<TModel> manager) where TModel: class, IModel, new()
		{
			return new ReadonlyConnectionManager<TModel> (manager);
		}
	}

	sealed class ReadonlyConnectionManager<TModel> : IReadonlyConnectionManager<TModel> where TModel : class, IModel, new()
	{
		readonly IReadonlyConnectionManager<TModel> _wrapped;



		public ReadonlyConnectionManager (IReadonlyConnectionManager<TModel> manager)
		{
			_wrapped = manager;
		}



		public async Task<ICollection<string>> ConnectionsForModel (int modelId)
		{
			return await _wrapped.ConnectionsForModel (modelId);
		}
	}
}

