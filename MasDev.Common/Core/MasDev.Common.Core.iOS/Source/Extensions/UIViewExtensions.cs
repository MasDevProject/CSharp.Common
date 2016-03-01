using System;
using UIKit;
using Foundation;
using CoreGraphics;

namespace MasDev.iOS.Extensions
{
	public static class UIViewExtensions
	{
		public static void InvertYAxis(this UIView view)
		{
			if (view == null)
				return;

			view.Transform = CGAffineTransform.MakeScale (1, -1);
		}

		public static void InvertXAxis(this UIView view)
		{
			if (view == null)
				return;

			view.Transform = CGAffineTransform.MakeScale (-1, 1);
		}

		public static void ToCircleView(this UIView view)
		{
			if (view == null)
				return;

			var maxDimension = (float) Math.Max (view.Bounds.Height, view.Bounds.Width);

			view.Layer.CornerRadius = maxDimension / 2;
			view.ClipsToBounds = true;
		}

		public static void HideKeyBoardOnTap(this UIView view)
		{
			if (view == null)
				return;
			
			var tap = new UITapGestureRecognizer { CancelsTouchesInView = false };
			tap.AddTarget(() => view.EndEditing(true));
			tap.ShouldReceiveTouch = (recognizer, touch) => 
				!(touch.View is UIControl || touch.View.FindSuperviewOfType(view, typeof(UITableViewCell)) != null);
			view.AddGestureRecognizer(tap);
		}

		/// <summary>
		/// Find the first responder in the <paramref name="view"/>'s subview hierarchy
		/// </summary>
		/// <param name="view">
		/// A <see cref="UIView"/>
		/// </param>
		/// <returns>
		/// A <see cref="UIView"/> that is the first responder or null if there is no first responder
		/// </returns>
		public static UIView FindFirstResponder(this UIView view)
		{
			if (view == null)
				return null;
			
			if (view.IsFirstResponder)
			{
				return view;
			}
			foreach (UIView subView in view.Subviews)
			{
				var firstResponder = subView.FindFirstResponder();
				if (firstResponder != null)
					return firstResponder;
			}
			return null;
		}

		/// <summary>
		/// Find the first Superview of the specified type (or descendant of)
		/// </summary>
		/// <param name="view">
		/// A <see cref="UIView"/>
		/// </param>
		/// <param name="stopAt">
		/// A <see cref="UIView"/> that indicates where to stop looking up the superview hierarchy
		/// </param>
		/// <param name="type">
		/// A <see cref="Type"/> to look for, this should be a UIView or descendant type
		/// </param>
		/// <returns>
		/// A <see cref="UIView"/> if it is found, otherwise null
		/// </returns>
		public static UIView FindSuperviewOfType(this UIView view, UIView stopAt, Type type)
		{
			if (view.Superview != null)
			{
				if (type.IsInstanceOfType(view.Superview))
				{
					return view.Superview;
				}

				if (view.Superview != stopAt)
					return view.Superview.FindSuperviewOfType(stopAt, type);
			}

			return null;
		}

		public static UIView FindTopSuperviewOfType(this UIView view, UIView stopAt, Type type)
		{
			var superview = view.FindSuperviewOfType(stopAt, type);
			var topSuperView = superview;
			while (superview != null && superview != stopAt)
			{
				superview = superview.FindSuperviewOfType(stopAt, type);
				if (superview != null)
					topSuperView = superview;

			}
			return topSuperView;
		}

		public static UIMotionEffect SetParallaxIntensity(this UIView view, float parallaxDepth, float? verticalDepth = null) {
			if (UIDevice.CurrentDevice.CheckSystemVersion(7, 0))
			{
				float vertical = verticalDepth ?? parallaxDepth;

				var verticalMotionEffect = new UIInterpolatingMotionEffect("center.y", UIInterpolatingMotionEffectType.TiltAlongVerticalAxis);
				verticalMotionEffect.MinimumRelativeValue = new NSNumber(-vertical);
				verticalMotionEffect.MaximumRelativeValue = new NSNumber(vertical);

				var horizontalMotionEffect = new UIInterpolatingMotionEffect("center.x", UIInterpolatingMotionEffectType.TiltAlongHorizontalAxis);
				horizontalMotionEffect.MinimumRelativeValue = new NSNumber(-parallaxDepth);
				horizontalMotionEffect.MaximumRelativeValue = new NSNumber(parallaxDepth);

				var group = new UIMotionEffectGroup();
				group.MotionEffects = new UIMotionEffect[]{ horizontalMotionEffect, verticalMotionEffect };

				view.AddMotionEffect(group);

				return group;
			}
			return null;
		}

		public static void FadeIn(this UIView view, double duration)
		{
			if (view == null)
				return;

			view.Alpha = 0;

			UIView.Animate (duration, () => {
				view.Alpha = 1;
			});
		}

		public static void FadeOut(this UIView view, double duration)
		{
			if (view == null)
				return;

			UIView.Animate (duration, () => {
				view.Alpha = 0;
				view.RemoveFromSuperview();
			});
		}
	}
}