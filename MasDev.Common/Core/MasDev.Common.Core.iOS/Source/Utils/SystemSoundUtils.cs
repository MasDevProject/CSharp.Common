using AudioToolbox;

namespace MasDev.iOS.Utils
{
	public static class SystemSoundUtils
	{
		public static void Vibrate(int milliseconds = 0)
		{
			//TODO: handle timer
			SystemSound.Vibrate.PlaySystemSound ();
		}
	}
}