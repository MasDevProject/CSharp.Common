using Android.Content;
using Android.Util;
using Android.Views;
using Android.Animation;
using Android.App;
using System;
using MasDev.Droid.ExtensionMethods;
using MasDev.Droid.Utils;
using Android.Widget;

namespace MasDev.Droid.Views
{
	public class ToolbarSearchView : Android.Support.V7.Widget.Toolbar
	{
		ProgressBar _pb;
		Android.Support.V7.Widget.SearchView _searchView;
		public Activity Activity { get; set; }
		const int ANIMATION_DURATION = 260;
		const string ANIMATION_NAME = "alpha";
		const int ALFA_MIN = 0;
		const int ALFA_MAX = 1;

		public Android.Support.V4.Widget.CursorAdapter SuggestionsAdapter { 
			get { return _searchView.SuggestionsAdapter; } 
			set { _searchView.SuggestionsAdapter = value; } 
		}

		public ToolbarSearchView (Context context) : this (context, null, 0) {}

		public ToolbarSearchView (Context context, IAttributeSet attrs) : this (context, attrs, 0) {}

		public ToolbarSearchView (Context context, IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle)
		{
			Init ();
		}

		void Init ()
		{
			SetNavigationOnClickListener (new NavigationOnClickListener (() => CloseSearchView()));

			_pb = new ProgressBar (Context);
			_pb.Indeterminate = true;
			_pb.SetInvisible ();
			var pad = ApplicationUtils.ConvertDpToPixel (Context, 5);
			//_pb.SetPadding (pad, pad, pad, pad);
			AddView (_pb);

			_searchView = new Android.Support.V7.Widget.SearchView (Context);
			AddView (_searchView);
		}

		public void OpenSearchView ()
		{
			this.SetVisible ();
			Alpha = ALFA_MIN;
			Animate ().Alpha (ALFA_MAX).SetDuration (ANIMATION_DURATION).Start ();
			_searchView.SetIconifiedByDefault (false);
			_searchView.RequestFocus ();
			ApplicationUtils.ShowKeyboard (Activity);
		}
			
		public bool CloseSearchView ()
		{
			ApplicationUtils.HideKeyboard (Activity);
			if (Visibility != ViewStates.Visible)
				return false;

			var anim = ObjectAnimator.OfFloat (this, ANIMATION_NAME, ALFA_MAX, ALFA_MIN).SetDuration (ANIMATION_DURATION);
			anim.AnimationEnd += (sender, e) => this.SetGone ();
			anim.Start ();
			return true;
		}

		public bool IsLoading {
			get {
				return _pb.Visibility == ViewStates.Visible;
			}
			set { 
				if (value)
					_pb.SetVisible ();
				else
					_pb.SetInvisible ();
			}
		}

		public void SubscribeToQueryTextSubmit (EventHandler<Android.Support.V7.Widget.SearchView.QueryTextSubmitEventArgs> evt)
		{
			_searchView.QueryTextSubmit += evt;
		}

		public void SubscribeToQueryTextChange(EventHandler<Android.Support.V7.Widget.SearchView.QueryTextChangeEventArgs> evt)
		{
			_searchView.QueryTextChange += evt;
		}
		public void SubscribeToSuggestionClick (EventHandler<Android.Support.V7.Widget.SearchView.SuggestionClickEventArgs> evt)
		{
			_searchView.SuggestionClick += evt;
		}

		protected override void OnDetachedFromWindow ()
		{
			base.OnDetachedFromWindow ();
			CloseSearchView ();
		}
	}
}

