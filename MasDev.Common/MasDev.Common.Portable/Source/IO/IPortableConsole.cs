
namespace MasDev.Common.IO
{
	public interface IPortableConsole
	{
		void WriteLine (string format, params object[] arguments);



		void WriteLine (object obj);

	}
}

