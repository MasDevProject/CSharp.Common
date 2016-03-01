using System;
using MasDev.iOS.App.ViewControllers;
using UIKit;
using Foundation;
using MasDev.Patterns.Injection;

namespace MasDev.Common
{
	//TODO: move to IViewModel declaration
	public enum ViewModelState
	{
		Normal,
		Loading,
		Error,
		Empty,
	}

	public class BaseViewController : ScrollableViewController
	{
		protected virtual string ViewTitle { get { return string.Empty; } }

		protected bool IsTablet { get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad; } }

		protected BaseViewController (IntPtr handle) : base (handle)
		{
		}

		protected BaseViewController (string nibName, NSBundle bundle) : base (nibName, bundle)
		{
		}

		protected BaseViewController (string nibName) : base (nibName, null)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			if(NavigationItem != null && !string.IsNullOrWhiteSpace(ViewTitle))
				NavigationItem.Title = ViewTitle;
		}

		// Utils

		protected void PerformSegue(string segue)
		{
			PerformSegue (segue, this);
		}

		// Utils

		protected virtual void ChangeState(ViewModelState state)
		{
			switch (state)
			{
			case ViewModelState.Empty:
				State = UIViewState.Empty;
				break;
			case ViewModelState.Error:
				State = UIViewState.Error;
				break;
			case ViewModelState.Loading:
				State = UIViewState.Loading;
				break;
			case ViewModelState.Normal:
				State = UIViewState.Normal;
				break;
			default:
				State = UIViewState.Normal;
				break;
			}
		}
	}

	public class BaseViewController<TViewModel> : BaseViewController where TViewModel : class, IViewModel
	{
		protected virtual TViewModel ViewModel { get; }

		protected BaseViewController (string nibName) : base(nibName)
		{
			ViewModel = Injector.Resolve<TViewModel> ();
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			ViewModel.SubscribeEvents ();
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			ViewModel.UnsubscribeEvents ();
		}
	}
}