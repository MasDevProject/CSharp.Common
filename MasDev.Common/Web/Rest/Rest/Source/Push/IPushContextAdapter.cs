

namespace MasDev.Rest.Push
{
	public interface IPushContextAdapter
	{
		IPushClients Push<T> () where T : PushManager;
	}
}

