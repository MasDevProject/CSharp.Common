using System;
using System.Collections.Generic;
using System.Linq;

namespace MasDev.Droid.Utils
{
	public sealed class AlphabetSectionsIndexer<T>
	{
		string[] _sections;
		Dictionary<string, int> _alphaIndexer;
		readonly Java.Lang.Object[] _sectionsObjects;
		IList<T> _items;
		Func<T, string> _itemConverter;

		public AlphabetSectionsIndexer (IList<T> objects, Func<T, string> itemConverter)
		{
			_items = objects;
			_alphaIndexer = new Dictionary<string, int> ();
			_itemConverter = itemConverter;

			for (var i = 0; i < objects.Count; i++)
			{
				var key = itemConverter.Invoke (objects [i]).Substring (0, 1).ToUpper ();
				if (!_alphaIndexer.ContainsKey (key))
					_alphaIndexer.Add (key, i);
			}

			_sections = _alphaIndexer.Keys.ToArray ();
			_sectionsObjects = new Java.Lang.Object[_sections.Length];

			for (var i = 0; i < _sections.Length; i++)
			{
				_sectionsObjects [i] = new Java.Lang.String (_sections [i]); 
			}
		}

		public int GetPositionForSection (int section)
		{ 
			return section >= _sections.Length ? _items.Count : _alphaIndexer [_sections [section]];
		}

		public int GetSectionForPosition (int position)
		{
			if (position >= _items.Count)
				return _sectionsObjects.Length;
			if (position <= 0)
				return 0; 

			var letter = _itemConverter (_items [position]).Substring (0, 1).ToUpper ();
			return _alphaIndexer.ContainsKey (letter) ? _alphaIndexer.Keys.ToList ().IndexOf (letter) : 0;
		}

		public Java.Lang.Object[] GetSections ()
		{
			return _sectionsObjects;
		}
	}
}

