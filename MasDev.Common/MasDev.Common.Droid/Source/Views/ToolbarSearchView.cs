using Android.Support.V7.Widget;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Animation;
using MasDev.Common.Droid.ExtensionMethods;
using MasDev.Common.Droid.Utils;
using Android.App;
using System;
using Android.Support.V4.Widget;

namespace MasDev.Common.Droid.Views
{
	public class ToolbarSearchView : Toolbar
	{
		SearchView _searchView;
		public Activity Activity { get; set; }
		const int ANIMATION_DURATION = 260;
		const string ANIMATION_NAME = "alpha";
		const int ALFA_MIN = 0;
		const int ALFA_MAX = 1;

		public CursorAdapter SuggestionsAdapter { 
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
			_searchView = new SearchView (Context);
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

		public void SubscribeToQueryTextSubmit (EventHandler<SearchView.QueryTextSubmitEventArgs> evt)
		{
			_searchView.QueryTextSubmit += evt;
		}

		public void SubscribeToQueryTextChange(EventHandler<SearchView.QueryTextChangeEventArgs> evt)
		{
			_searchView.QueryTextChange += evt;
		}
		public void SubscribeToSuggestionClick (EventHandler<SearchView.SuggestionClickEventArgs> evt)
		{
			_searchView.SuggestionClick += evt;
		}
	}
}

