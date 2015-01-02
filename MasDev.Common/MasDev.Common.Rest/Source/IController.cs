
namespace MasDev.Rest
{
	public interface IController<TModule> : IHttpContext where TModule : Module
	{
		TModule Module { get; }
	}
}

