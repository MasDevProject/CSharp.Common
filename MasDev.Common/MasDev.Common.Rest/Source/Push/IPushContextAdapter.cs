

namespace MasDev.Common.Rest.Push
{
	public interface IPushContextAdapter
	{
		IPushClients Push<T> () where T : PushManager;
	}
}

