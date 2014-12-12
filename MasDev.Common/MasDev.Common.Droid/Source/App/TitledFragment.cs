using Android.Views;
using System;
using Android.Support.V4.App;
using Android.Animation;
using MasDev.Common.Droid.ExtensionMethods;


namespace MasDev.Common.Droid.App
{
	public enum UiState
	{
		Success,
		Failure,
		Loading,
		NoResults
	}

	public abstract class TitledFragment : DialogFragment, View.IOnTouchListener
	{
		public string Title { get; set; }

		public bool IsRootFragment { get; set; }

		public virtual bool OnKeyUp (Keycode keyCode, KeyEvent e)
		{
			return false;
		}

		bool View.IOnTouchListener.OnTouch (View v, MotionEvent e)
		{
			return true;
		}

		protected class NavigationOnClickListener : Java.Lang.Object, View.IOnClickListener
		{
			readonly Action _onClick;

			public NavigationOnClickListener(Action onclick)
			{
				_onClick = onclick;
			}

			public void OnClick (View v)
			{
				if (_onClick != null)
					_onClick.Invoke ();
			}
		}

		#region View states managment

		protected View LoadingLayout { set; get; }

		protected View ErrorLayout { get; set; }

		protected View ContentLayout { set; get; }

		protected View NoResultsLayout { set; get; }

		UiState? _uiState;
		const int ANIMATION_DURATION = 400;
		const string ALPHA = "alpha";
		bool _useAnimation;


		protected void InitStates (View rootView, int? loadingViewId, int? failureViewId, int? successFulViewId, int? noResultsViewId, bool useAnimation = true)
		{
			_useAnimation = useAnimation;
			LoadingLayout = loadingViewId != null ? rootView.FindViewById (loadingViewId.Value) : null;
			ErrorLayout = failureViewId != null ? rootView.FindViewById (failureViewId.Value) : null;
			ContentLayout = successFulViewId != null ? rootView.FindViewById (successFulViewId.Value) : null;
			NoResultsLayout = noResultsViewId != null ? rootView.FindViewById (noResultsViewId.Value) : null;

			if (LoadingLayout != null)
				LoadingLayout.SetVisible ();
			if (ErrorLayout != null)
				ErrorLayout.SetInvisible ();
			if (ContentLayout != null)
				ContentLayout.SetInvisible ();
			if (NoResultsLayout != null)
				NoResultsLayout.SetInvisible ();
		}



		protected UiState UiState {
			get { return _uiState.Value; }
			set { 
				if (_uiState == value)
					return;

				_uiState = value;
				switch (_uiState) {
				case UiState.Success:
					ShowSuccessFulUi ();
					break;
				case UiState.Loading:
					ShowLoadingUi ();
					break;
				case UiState.Failure:
					ShowErrorUi ();
					break;
				case UiState.NoResults:
					ShowNoResultsUi ();
					break;
				}
			}
		}


		void ShowLoadingUi ()
		{
			if (LoadingLayout == null)
				throw new InvalidOperationException ("LoadingView is not initialized! (check the InitStates() method)");

			FadeIn (LoadingLayout);
			FadeOut (ErrorLayout);
			FadeOut (NoResultsLayout);
			FadeOut (ContentLayout);
		}

		void ShowErrorUi ()
		{
			if (ErrorLayout == null)
				throw new InvalidOperationException ("FailureView is not initialized! (check the InitStates() method)");

			FadeIn (ErrorLayout);
			FadeOut (LoadingLayout);
			FadeOut (NoResultsLayout);
			FadeOut (ContentLayout);
		}

		void ShowSuccessFulUi ()
		{
			if (ContentLayout == null)
				throw new InvalidOperationException ("SuccessView is not initialized! (check the InitStates() method)");

			FadeIn (ContentLayout);
			FadeOut (LoadingLayout);
			FadeOut (NoResultsLayout);
			FadeOut (ErrorLayout);
		}

		void ShowNoResultsUi ()
		{
			if (NoResultsLayout == null)
				throw new InvalidOperationException ("NoResultsView is not initialized! (check the InitStates() method)");

			FadeIn (NoResultsLayout);
			FadeOut (LoadingLayout);
			FadeOut (ContentLayout);
			FadeOut (ErrorLayout);
		}


		void FadeIn (View layoutToShow)
		{
			if (layoutToShow.Visibility == ViewStates.Visible && !(layoutToShow.Tag != null && (bool)layoutToShow.Tag))
				return;

			layoutToShow.Tag = false;
			if (_useAnimation) {
				var an = ObjectAnimator.OfFloat (layoutToShow, ALPHA, 0f, 1f).SetDuration (ANIMATION_DURATION);
				an.Start ();
			}
			layoutToShow.SetVisible ();
		}

		void FadeOut (View layoutToHie)
		{
			if (layoutToHie == null || layoutToHie.Visibility != ViewStates.Visible)
				return;
				
			if (_useAnimation) {
				layoutToHie.Tag = true;
				var an = ObjectAnimator.OfFloat (layoutToHie, ALPHA, 1f, 0f).SetDuration (ANIMATION_DURATION);
				an.AnimationEnd += delegate {
					if ((bool)layoutToHie.Tag) {
						layoutToHie.SetInvisible ();
					}
				};
				an.Start ();
			} else
				layoutToHie.SetGone ();
		}

		#endregion
	}
}

