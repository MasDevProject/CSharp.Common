using Android.Views;
using System;
using Android.Animation;
using MasDev.Droid.ExtensionMethods;
using Android.App;
using MasDev.Droid.Utils;


namespace MasDev.Droid.App
{
	public enum UiState
	{
		Success,
		Failure,
		Loading,
		NoResults
	}

	public abstract class TitledFragment : Android.Support.V4.App.DialogFragment, View.IOnTouchListener
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

		#region View states managment

		protected View LoadingLayout { get; private set; }

		protected View ErrorLayout { get; private set; }

		protected View ContentLayout { get; private set; }

		protected View NoResultsLayout { get; private set; }

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

			layoutToShow.Tag = false; // isHiding = false
			if (_useAnimation)
				ObjectAnimator.OfFloat (layoutToShow, ALPHA, 0f, 1f).SetDuration (ANIMATION_DURATION).Start ();

			layoutToShow.SetVisible ();
		}

		void FadeOut (View layoutToHide)
		{
			if (layoutToHide == null || layoutToHide.Visibility != ViewStates.Visible)
				return;

			if (_useAnimation) {
				layoutToHide.Tag = true; // isHiding = true
				var an = ObjectAnimator.OfFloat (layoutToHide, ALPHA, 1f, 0f).SetDuration (ANIMATION_DURATION);
				an.AnimationEnd += delegate {
					if ((bool)layoutToHide.Tag) { // if the layout is set to Visible during the animation, I don't want to set it as Invisible when the animation ends.
						layoutToHide.SetInvisible ();
					}
				};
				an.Start ();
			} else
				layoutToHide.SetGone ();
		}

		#endregion
	}
		
	public abstract class TitledFragment<TParent> : TitledFragment where TParent : class
	{
		protected TParent Parent { get; set; }

		public override void OnAttach (Activity activity)
		{
			base.OnAttach (activity);
			if (Parent == null)
				Parent = FragmentUtils.EnsureImplements<TParent> (this);
		}
	}
}

