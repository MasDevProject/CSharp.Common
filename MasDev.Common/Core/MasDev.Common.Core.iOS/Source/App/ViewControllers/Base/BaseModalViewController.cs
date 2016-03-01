﻿using System;
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
		protected virtual TViewModel ViewModel { get; }

		protected BaseModalViewController (string nibName) : base(nibName)
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