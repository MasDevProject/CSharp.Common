using MasDev.Utils;


namespace MasDev.Droid.Utils
{
	public class Logger : ILogger
	{
		public void Log (string tag, object message)
		{
			Android.Util.Log.Debug (tag, message.ToString ());
		}
		public void Log (object message)
		{
			Android.Util.Log.Debug (string.Empty, message.ToString ());
		}
	}
}

