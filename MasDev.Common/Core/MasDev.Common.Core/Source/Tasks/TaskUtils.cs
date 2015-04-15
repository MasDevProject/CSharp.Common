using System.Threading.Tasks;

namespace MasDev.Threading.Tasks
{
	public static class TaskUtils
	{
		public readonly static Task Void = Task.FromResult (0);

		public static Task<T> GetVoid<T> ()
		{
			return Task.FromResult<T> (default(T));
		}
	}
}

