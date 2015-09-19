using UIKit;

namespace MasDev.iOS.Utils
{
	public static class UIColorUtils
	{
		public static UIColor FromHex(string hexValue)
		{
			hexValue = hexValue.Replace ("#", string.Empty);

			int colValue = int.Parse (hexValue,
				System.Globalization.NumberStyles.HexNumber);
			return UIColor.FromRGB(
				(((float)((colValue & 0xFF0000) >> 16))/255.0f),
				(((float)((colValue & 0xFF00) >> 8))/255.0f),
				(((float)(colValue & 0xFF))/255.0f));
		}
	}
}