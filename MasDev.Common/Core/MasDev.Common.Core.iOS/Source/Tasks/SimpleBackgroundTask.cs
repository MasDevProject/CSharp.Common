using System;
using System.Threading.Tasks;
using UIKit;

namespace MasDev.Common
{
	public static class SimpleBackgroundTask
	{
		const int EmptyTaskId = -1;

		static nint taskId = EmptyTaskId;

		public static async void StartTask (Func<Task> taskFunction)
		{
			taskId = UIApplication.SharedApplication.BeginBackgroundTask (OnExpire);

			try
			{
				await Task.Run (async () => await taskFunction.Invoke());
			}
			finally 
			{
				if (taskId != EmptyTaskId)
					UIApplication.SharedApplication.EndBackgroundTask (taskId);

				taskId = EmptyTaskId;
			}
		}

		static void OnExpire()
		{
			if(taskId != EmptyTaskId)
				UIApplication.SharedApplication.EndBackgroundTask (taskId);

			taskId = EmptyTaskId;
		}
	}
}