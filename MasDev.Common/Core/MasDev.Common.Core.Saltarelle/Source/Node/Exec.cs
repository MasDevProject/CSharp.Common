using System.Threading.Tasks;
using NodeJS.ChildProcessModule;


namespace MasDev.Common.Saltarelle
{
	public static class Exec
	{
		public static Task<string> Command (string command)
		{
			var tcs = new TaskCompletionSource<string> ();
			ChildProcess.Exec (command, (error, stdout, stderr) => tcs.SetResult (stdout.ToString ()));
			return tcs.Task;
		}
	}
}

