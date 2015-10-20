using UIKit;

namespace MasDev.iOS.Views.Fragments
{
	public interface IFragmentView
	{
		string Title { get; }

		UIView View { get; }

		/// <summary>
		/// Create this instance.
		/// </summary>
		void Create();

		/// <summary>
		/// Show this instance.
		/// </summary>
		void Show();

		/// <summary>
		/// Cleanup this instance.
		/// </summary>
		void Cleanup();

		void Refresh();
	}
}