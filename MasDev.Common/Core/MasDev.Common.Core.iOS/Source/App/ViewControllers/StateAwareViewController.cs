using System;
using UIKit;
using MasDev.iOS.Views.States;
using Foundation;

namespace MasDev.iOS.App.ViewControllers
{
	public class StateAwareViewController : UIViewController
	{
		UIViewState _viewState;

		protected UIView LoadingStateView;
		protected UIView ErrorStateView;
		protected UIView EmptyStateView;

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
			LoadingStateView = new BaseLoadingStateView (View.Bounds);
			ErrorStateView = new BaseErrorStateView (View.Bounds, null, string.Empty);
			EmptyStateView = new BaseEmptyStateView (View.Bounds, string.Empty);
		}

		protected virtual void RefreshViewState()
		{
			HandleState (UIViewState.Empty, EmptyStateView);
			HandleState (UIViewState.Error, ErrorStateView);
			HandleState (UIViewState.Loading, LoadingStateView);
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