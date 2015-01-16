using Android.OS;
using Android.Views;
using System.Collections.Generic;
using Android.Views.Animations;
using MasDev.Utils;

namespace Nirhart.ParallaxScroll.Views
{
	public abstract class ParallaxedView
	{
		readonly public static bool IsAPI11 = Build.VERSION.SdkInt >= BuildVersionCodes.Honeycomb;
		protected WeakReferenceEx<View> _view;
		protected int _lastOffset;
		protected List<Animation> _animations;

		protected abstract void TranslatePreICS (View view, float offset);

		protected ParallaxedView (View view)
		{
			_lastOffset = 0;
			_animations = new List<Animation> ();
			_view = new WeakReferenceEx<View> (view);
		}

		#region Offset

		float _offset;

		public float Offset {
			set {
				_offset = value;
				View view = _view.Target;
				if (view != null)
				if (IsAPI11) {
					view.TranslationY = value;
				} else {
					TranslatePreICS (view, value);
				}
			}

			get {
				return _offset;
			}
		}

		#endregion

		#region Alpha

		float _alpha;

		public float Alpha {
			set {
				_alpha = value;
				View view = _view.Target;
				if (view == null)
					return;

				if (IsAPI11) {
					view.Alpha = value;
				} else {
					AlphaPreICS (value);
				}
			}

			get {
				return _alpha;
			}
		}

		#endregion

		public View View {
			set {
				_view = new WeakReferenceEx<View> (value);
			}
		}

		public bool Is (View v)
		{
			return (v != null && _view != null && _view.Target != null && _view.Target.Equals (v));
		}

		readonly object _locker = new object ();
		internal void AddAnimation (Animation animation)
		{
			lock (_locker) {
				_animations.Add (animation);
			}
		}

		internal void AlphaPreICS (float alpha)
		{
			AddAnimation (new AlphaAnimation (alpha, alpha));
		}

		internal void AnimateNow ()
		{
			lock (_locker) {
				View view = _view.Target;
				if (view == null)
					return;

				var set = new AnimationSet (true);
				foreach (var animation in _animations) {
					if (animation != null)
						set.AddAnimation (animation);
				}
				set.Duration = 0;
				set.FillAfter = true;
				view.Animation = set;
				set.Start ();
				_animations.Clear ();
			}
		}
	}
}
