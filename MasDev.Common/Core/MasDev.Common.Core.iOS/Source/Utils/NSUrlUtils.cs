using System;

namespace MasDev.iOS.Utils
{
	public static class NSUrlUtils
	{
		const string CallAction = "tel:";
		const string MailAction = "mailto:";
		const string AppleMapsAction = "http://maps.apple.com?q=";
		const string WebAction = "http://";

		public enum ActionEnum
		{
			NONE,
			CALL,
			TEXT,
			MAPS,
			WEB,
			MAIL,
		}

		public static string UrlFromAction(ActionEnum action)
		{
			var result = String.Empty;

			switch (action) 
			{
			case ActionEnum.CALL:
				result = CallAction;
				break;
			case ActionEnum.MAIL:
				result = MailAction;
				break;
			case ActionEnum.MAPS:
				result = AppleMapsAction;
				break;
			case ActionEnum.WEB:
				result = WebAction;
				break;
			}

			return result;
		}
	}
}