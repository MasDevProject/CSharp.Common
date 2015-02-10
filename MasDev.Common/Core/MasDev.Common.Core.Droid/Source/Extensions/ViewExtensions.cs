using Android.Views;
using Android.Views.Animations;
using System.Collections.Generic;

namespace MasDev.Droid.ExtensionMethods
{
	public static class ViewExtensions
	{
		public static void SetGone (this View view)
		{
			view.Visibility = ViewStates.Gone;
		}

		public static void SetInvisible (this View view)
		{
			view.Visibility = ViewStates.Invisible;
		}

		public static void SetVisible (this View view)
		{
			view.Visibility = ViewStates.Visible;
		}

		public static ScaleAnimation Expand (this View v, int durationInMillis = 400)
		{
			v.Visibility = ViewStates.Visible;
			var anim = new ScaleAnimation(1, 1, 0, 1);
			anim.Duration = durationInMillis;
			v.StartAnimation (anim);
			return anim;
		}

		public static ScaleAnimation Collapse (this View v, int durationInMillis = 400)
		{
			var anim = new ScaleAnimation(1, 1, 1, 0);
			anim.Duration = durationInMillis;
			anim.AnimationEnd += (s, e) => v.Visibility = ViewStates.Gone;
			v.StartAnimation (anim);
			return anim;
		}

		public static IEnumerable<View> Children (this ViewGroup v)
		{
			for (var i = 0; i < v.ChildCount; i++)
				yield return v.GetChildAt (i);
		}
	}
}

