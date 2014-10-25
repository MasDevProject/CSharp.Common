
namespace MasDev.Common
{
	public interface ILogger
	{
		void Log (string tag, object message);

		void Log (object message);
	}
}

