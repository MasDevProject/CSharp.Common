using UIKit;

namespace MasDev.iOS.Utils
{
	public interface IDialogManager
	{
		bool IsShowing { get; }

		void Show(UIView view, string message);

		void Show(UIView view, string message, bool animated);

		//void ShowMessage(UIView view, string message);

		//void ShowMessage(UIView view, string message, bool animated);

		void Dismiss();

		void Dismiss(bool animated);

		void Dismiss(bool animated, double delay);
	}
}