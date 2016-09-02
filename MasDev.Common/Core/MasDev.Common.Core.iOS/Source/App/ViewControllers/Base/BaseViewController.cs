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

	public interface IParent { }

	public class BaseViewController : ScrollableViewController
	{
		protected virtual string ViewTitle { get { return string.Empty; } }

		protected bool IsTablet { get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad; } }

		protected bool IsLandscape
		{
			get 
			{
				var interfaceOrientation = UIApplication.SharedApplication.StatusBarOrientation;
				var interfaceIsLandscape = interfaceOrientation == UIInterfaceOrientation.LandscapeLeft ||
					interfaceOrientation == UIInterfaceOrientation.LandscapeRight;

				return UIDevice.CurrentDevice.Orientation.IsLandscape () || interfaceIsLandscape;
			}
		}

		protected BaseViewController (IntPtr handle) : base (handle)
		{
		}

		protected BaseViewController (string nibName, NSBundle bundle) : base (nibName, bundle)
		{
		}

		protected BaseViewController (string nibName) : base (nibName, null)
		{
		}

		// Lifecycle

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			if(NavigationItem != null && !string.IsNullOrWhiteSpace(ViewTitle))
				NavigationItem.Title = ViewTitle;

			OnCreate ();
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			if (IsMovingFromParentViewController || IsBeingDismissed)
				OnDestroy ();
		}

		public override void RemoveFromParentViewController ()
		{
			base.RemoveFromParentViewController ();

			OnDestroy ();
		}

		protected virtual void OnCreate()
		{
		}

		protected virtual void OnDestroy()
		{
		}

		// Utils

		protected void PerformSegue(string segue)
		{
			PerformSegue (segue, this);
		}

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
		protected virtual TViewModel ViewModel { get; private set; }

		protected BaseViewController (string nibName) : base(nibName)
		{
			Initialize ();
		}

		protected BaseViewController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}

		protected BaseViewController (string nibName, NSBundle bundle) : base (nibName, bundle)
		{
			Initialize ();
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

		// Utils

		protected void Initialize()
		{
			ViewModel = Injector.Resolve<TViewModel> ();
		}
	}

	public class BaseViewControllerWithParent<TParent> : BaseViewController where TParent : class, IParent
	{
		protected TParent Parent;

		protected BaseViewControllerWithParent (string nibName) : base(nibName)
		{
		}

		protected BaseViewControllerWithParent (IntPtr handle) : base (handle)
		{
		}

		protected BaseViewControllerWithParent (string nibName, NSBundle bundle) : base (nibName, bundle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Parent = this.GetParent<TParent> ();
		}
	}

	public class BaseViewController<TViewModel, TParent> : BaseViewController<TViewModel> where TViewModel : class, IViewModel where TParent : class, IParent
	{
		protected TParent Parent;

		protected BaseViewController (string nibName) : base(nibName)
		{
		}

		protected BaseViewController (IntPtr handle) : base (handle)
		{
		}

		protected BaseViewController (string nibName, NSBundle bundle) : base (nibName, bundle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Parent = this.GetParent<TParent> ();
		}
	}
}