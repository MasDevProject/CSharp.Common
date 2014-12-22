using Android.OS;
using System;
using Android.Views;
using System.Collections.Generic;
using Android.Views.Animations;
using MasDev.Common.Utils;

namespace Nirhart.ParallaxScroll.Views
{
	public abstract class ParallaxedView
	{
		readonly public static bool IsAPI11 = Build.VERSION.SdkInt >= Build.VERSION_CODES.Honeycomb;
		protected WeakReferenceEx<View> _view;
		protected int _lastOffset;
		protected List<Animation> _animations;

		protected abstract void TranslatePreICS (View view, float offset);

		public ParallaxedView (View view)
		{
			this._lastOffset = 0;
			this._animations = new List<Animation> ();
			this._view = new WeakReferenceEx<View> (view);
		}

		#region Offset

		private float _offset;

		public float Offset {
			set {
				_offset = value;
				View view = this._view.Target;
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

		private float _alpha;

		public float Alpha {
			set {
				_alpha = value;
				View view = this._view.Target;
				if (view == null)
					return;

				if (IsAPI11) {
					view.Alpha = value;
				} else {
					AlphaPreICS (view, value);
				}
			}

			get {
				return _alpha;
			}
		}

		#endregion

		public View View {
			set {
				this._view = new WeakReferenceEx<View> (value);
			}
		}

		public bool Is (View v)
		{
			return (v != null && _view != null && _view.Target != null && _view.Target.Equals (v));
		}

		internal void AddAnimation (Animation animation)
		{
			lock (this) {
				_animations.Add (animation);
			}
		}

		internal void AlphaPreICS (View view, float alpha)
		{
			AddAnimation (new AlphaAnimation (alpha, alpha));
		}

		internal void AnimateNow ()
		{
			lock (this) {
				View view = this._view.Target;
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
