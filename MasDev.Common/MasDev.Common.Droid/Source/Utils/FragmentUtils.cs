using System;
using Android.Support.V4.App;

namespace MasDev.Droid.Utils
{
	public static class FragmentUtils
	{
		public static TInterface EnsureImplements<TInterface>(Fragment fragment) where TInterface : class
		{
			var parent = GetParent<TInterface> (fragment);
			if (parent == null)
				throw new NotSupportedException ("The parent activity/fragment must implement " + typeof(TInterface).FullName);

			return parent;
		}

		public static TInterface GetParent<TInterface>(Fragment fragment) where TInterface : class
		{
			var parent = fragment.Activity as TInterface;
			if (parent != null)
				return parent;

			parent = fragment.ParentFragment as TInterface;
			return parent;
		}
	}
}

