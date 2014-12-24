using System;
using Android.Content;
using Android.Support.V4.App;

namespace MasDev.Common.Droid.Utils
{
	public static class FragmentUtils
	{
		public static TInterface EnsureImplements<TInterface>(Context ctx, Fragment fragment = null) where TInterface : class
		{
			var parent = GetParent<TInterface> (ctx, fragment);
			if (parent == null)
				throw new NotSupportedException ("The parent activity/fragment must implement " + typeof(TInterface).FullName);

			return parent;
		}

		public static TInterface GetParent<TInterface>(Context ctx, Fragment fragment = null) where TInterface : class
		{
			var parent = ctx as TInterface;
			if (parent != null)
				return parent;

			parent = fragment.ParentFragment as TInterface;
			return parent;
		}
	}
}

