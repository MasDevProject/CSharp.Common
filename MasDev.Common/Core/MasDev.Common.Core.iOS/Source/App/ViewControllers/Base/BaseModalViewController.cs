using System;
using UIKit;
using Foundation;
using MasDev.Patterns.Injection;

namespace MasDev.Common
{
	public class BaseModalViewController : BaseViewController
	{
		protected UIBarButtonItem BtnCancel;
		protected UIBarButtonItem BtnDone;

		protected virtual bool ShowDoneButton { get { return false; } }
		protected virtual bool ShowCancelButton { get { return true; } }

		protected BaseModalViewController (string nibName, NSBundle bundle) : base (nibName, bundle)
		{
		}

		protected BaseModalViewController (IntPtr handle) : base (handle)
		{
		}

		protected BaseModalViewController (string nibName) : base (nibName, null)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			if (NavigationItem == null)
				return;

			if (ShowCancelButton) 
			{
				BtnCancel = new UIBarButtonItem (UIBarButtonSystemItem.Cancel);
				NavigationItem.SetLeftBarButtonItem (BtnCancel, true);
			}

			if (ShowDoneButton)
			{
				BtnDone = new UIBarButtonItem (UIBarButtonSystemItem.Done);
				NavigationItem.SetRightBarButtonItem (BtnDone, true);
			}
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			if(BtnCancel != null)
				BtnCancel.Clicked += CancelButtonClicked;

			if(BtnDone != null)
				BtnDone.Clicked += DoneButtonClicked;
		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			if(BtnCancel != null)
				BtnCancel.Clicked -= CancelButtonClicked;

			if(BtnDone != null)
				BtnDone.Clicked -= DoneButtonClicked;
		}

		void CancelButtonClicked (object sender, EventArgs e)
		{
			HandleCancelRequest ();
		}

		void DoneButtonClicked (object sender, EventArgs e)
		{
			HandleDoneRequest ();
		}

		protected void Dismiss()
		{
			DismissViewController (true, null);
		}

		protected virtual void HandleCancelRequest()
		{
			Dismiss ();
		}

		protected virtual void HandleDoneRequest()
		{
		}

		protected virtual void ToggleButtons(bool enabled)
		{
			ToggleButton (BtnCancel, enabled);
			ToggleButton (BtnDone, enabled);
		}

		void ToggleButton(UIBarButtonItem button, bool enabled)
		{
			if (button != null)
				button.Enabled = enabled;
		}
	}

	public class BaseModalViewController<TViewModel> : BaseModalViewController where TViewModel : class, IViewModel
	{
		protected virtual TViewModel ViewModel { get; private set; }

		protected BaseModalViewController (string nibName) : base(nibName)
		{
			Initialize ();
		}

		protected BaseModalViewController (string nibName, NSBundle bundle) : base (nibName, bundle)
		{
			Initialize ();
		}

		protected BaseModalViewController (IntPtr handle) : base (handle)
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

	public class BaseModalViewControllerWithParent<TParent> : BaseModalViewController where TParent : class, IParent
	{
		protected TParent Parent;

		protected BaseModalViewControllerWithParent (string nibName) : base(nibName)
		{
		}

		protected BaseModalViewControllerWithParent (IntPtr handle) : base (handle)
		{
		}

		protected BaseModalViewControllerWithParent (string nibName, NSBundle bundle) : base (nibName, bundle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Parent = this.GetParent<TParent> ();
		}
	}

	public class BaseModalViewController<TViewModel, TParent> : BaseModalViewController<TViewModel> where TViewModel : class, IViewModel where TParent : class, IParent
	{
		protected TParent Parent;

		protected BaseModalViewController (string nibName) : base(nibName)
		{
		}

		protected BaseModalViewController (IntPtr handle) : base (handle)
		{
		}

		protected BaseModalViewController (string nibName, NSBundle bundle) : base (nibName, bundle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			Parent = this.GetParent<TParent> ();
		}
	}
}