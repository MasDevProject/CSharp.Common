using AudioToolbox;
using System;
using UIKit;
using System.Threading.Tasks;

namespace MasDev.iOS.Utils
{
	public static class SystemSoundUtils
	{
		public static async void Vibrate(int milliseconds = 0)
		{
			if (milliseconds < 0)
				return;

			var diff = DateTime.Now;
			var supportsAsyncVibration = UIDevice.CurrentDevice.CheckSystemVersion (9, 0);

			if (supportsAsyncVibration)
				await SystemSound.Vibrate.PlaySystemSoundAsync ();
			else
				SystemSound.Vibrate.PlaySystemSound ();

			if (milliseconds <= 0)
				return;

			await Task.Delay(supportsAsyncVibration ? 400 : 800);

			Vibrate (milliseconds - (DateTime.Now - diff).Milliseconds);
		}
	}
}