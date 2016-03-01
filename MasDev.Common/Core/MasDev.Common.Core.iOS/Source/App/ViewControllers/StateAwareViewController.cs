using System;
using UIKit;
using MasDev.iOS.Views.States;
using Foundation;

namespace MasDev.iOS.App.ViewControllers
{
	public class StateAwareViewController : UIViewController
	{
		private UIViewState _viewState;

		protected UIView loadingStateView;
		protected UIView errorStateView;
		protected UIView emptyStateView;

		public UIViewState State
		{
			get { return _viewState; }
			set
			{
				if (_viewState == value)
					return;

				_viewState = value;

				RefreshViewState ();
			}
		}

		protected virtual UIView ContainerView
		{
			get { return View; }
		}

		protected StateAwareViewController (IntPtr handle) : base(handle)
		{
		}

		protected StateAwareViewController (string nibName, NSBundle bundle) : base(nibName, bundle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			InitializeViewStates ();
		}

		protected virtual void InitializeViewStates()
		{
			loadingStateView = new BaseLoadingStateView (View.Bounds);
			errorStateView = new BaseErrorStateView (View.Bounds, null, string.Empty);
			emptyStateView = new BaseEmptyStateView (View.Bounds, string.Empty);
		}

		protected virtual void RefreshViewState()
		{
			HandleState (UIViewState.Empty, emptyStateView);
			HandleState (UIViewState.Error, errorStateView);
			HandleState (UIViewState.Loading, loadingStateView);
		}

		void HandleState(UIViewState state, UIView stateView)
		{
			if (State == state)
				ShowView (stateView);
			else
				DismissView (stateView);
		}

		void ShowView(UIView view)
		{
			if (view != null) 
			{
				view.Frame = ContainerView.Bounds;
				view.Layer.CornerRadius = ContainerView.Layer.CornerRadius;

				ContainerView.AddSubview (view);
			}
		}

		void DismissView(UIView view)
		{
			if (view != null)
				view.RemoveFromSuperview ();
		}
	}

	public enum UIViewState
	{
		Normal,
		Loading,
		Error,
		Empty,
	}
}