using System;
using Android.Database;
using System.Collections.Generic;

namespace MasDev.Common.Droid
{
	public static class SearchViewUtils
	{
		/// <summary>
		/// Gets a simple cursor given an array of suggestions. This is necessary when you want to display a list of suggestions below a SearchView. (using AutocompleteTextView is simpler, but you can't use the same strategy in a SearchView)
		/// This is an example of usage (using a SearchView located on the ActionBar):
		/// 
		/// public override bool OnCreateOptionsMenu (IMenu menu)
		/// {
		/// var inflater = MenuInflater;
		/// inflater.Inflate (Resource.Menu.pizzerias_map_menu, menu);
		/// _actionBarSearchView = (SearchView)menu.FindItem (Resource.Id.search_location).ActionView;
		/// _actionBarSearchView.QueryTextChange += HandleQueryTextChange;
		/// return base.OnCreateOptionsMenu (menu);
		/// }
		/// 
		/// CursorAdapter ad;
		/// const string ADDRESS_SUGGESTION_COLUMN_NAME = "c";
		/// async void HandleQueryTextChange (object sender, SearchView.QueryTextChangeEventArgs e)
		/// {
		/// string[] _seggestions = await GetSuggestionsBlaBla();
		/// if (ad == null) {
		/// ad = new SimpleCursorAdapter (this, Android.Resource.Layout.SimpleListItem1, null, new [] {ADDRESS_SUGGESTION_COLUMN_NAME}, new []{ Android.Resource.Id.Text1}, 0);
		/// _actionBarSearchView.SuggestionsAdapter = ad;
		/// _actionBarSearchView.SuggestionClick += (s, evt) => System.Diagnostics.Debug.WriteLine (_seggestions [evt.Position]);
		/// }
		/// ad.ChangeCursor (GetSimleCursor(_seggestions, ADDRESS_SUGGESTION_COLUMN_NAME));
		/// }
		/// 
		/// </summary>
		/// <returns>The simle cursor.</returns>
		/// <param name="suggestions">Suggestions.</param>
		/// <param name="columnName">Column name.</param>
		/// <param name = "toString">tostring</param>
		public static ICursor GetSimpleCursor<T>(List<T> suggestions, string columnName, Func<T, string> toString)
		{
			var c = new MatrixCursor (new [] { "_id", columnName });
			long x = 0L;
			foreach(var s in suggestions)
				c.AddRow (new Java.Lang.Object[] { x++, toString(s) });

			return c;
		}
	}
}

