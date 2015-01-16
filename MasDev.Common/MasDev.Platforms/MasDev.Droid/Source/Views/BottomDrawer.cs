using Android.Widget;
using System;
using Android.Views;
using Android.Graphics;
using Android.Animation;
using Android.Content;
using Android.Util;
using MasDev.Extensions;


namespace MasDev.Droid.Views
{
	public class BottomDrawer : FrameLayout
	{
		public event Action OnDrawerOpen;
		public event Action OnDrawerClose;

		int _drawerClosedY;
		int _drawerOpenY;
		int _originalDrawerHeight;
		View _contentLayout;
		View _drawerLayout;
		View _shadowLayout;
		ObjectAnimator _slideUpAniamtion;
		ObjectAnimator _slideDownAniamtion;
		ObjectAnimator _fadeInAniamtion;
		ObjectAnimator _faceOutAniamtion;

		const string TRANSLATION_ANIMATION = "translationY";
		const string ALPHA_ANIMATION = "alpha";

		#region Public properties

		public bool IsOpen { get; private set; }

		int _animationDuration = 400;

		public int AnimationDurationMillis { get { return _animationDuration; } set { _animationDuration = value; } }

		public float DrawerCloseShadowAlpha { get; set; }

		float _drawerOpenShadowAlpha = 0.7f;

		public float DrawerOpenAlpha { get { return _drawerOpenShadowAlpha; } set { _drawerOpenShadowAlpha = value; } }

		bool _shadowEnabled = true;

		public bool ShadowEnabled { get { return _shadowEnabled; } set { _shadowEnabled = value; } }

		int _drawerCloseStartingY = -1;

		public int DrawerCloseStartingY { 
			get { 
				if (_drawerCloseStartingY == -1)
					_drawerCloseStartingY = (int)TypedValue.ApplyDimension (ComplexUnitType.Dip, 53, Context.Resources.DisplayMetrics); 
				return _drawerCloseStartingY;
			} 
			set { 
				_drawerCloseStartingY = value;
			} 
		}

		public int DrawerHeightPercentage { get; set; }

		#endregion

		public BottomDrawer (Context context) : base (context)
		{
		}

		public BottomDrawer (Context context, IAttributeSet attrs) : base (context, attrs)
		{
		}

		public BottomDrawer (Context context, IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle)
		{
		}

		bool _doIt = true;
		protected override void OnLayout (bool changed, int left, int top, int right, int bottom)
		{
			base.OnLayout (changed, left, top, right, bottom);

			if (!changed)
				return;

			if (!_doIt) {
				_doIt = true;
				return;
			}

			if (_shadowLayout == null)
				_shadowLayout = GetShadowLayout ();

			if (_contentLayout == null)
				_contentLayout = GetChildAt (0);

			if (IndexOfChild (_shadowLayout) == -1)
				AddView (_shadowLayout, 1);

			if (_drawerLayout == null)
				_drawerLayout = GetChildAt (2);

			if (ChildCount != 3)
				throw new Exception ("The bottomDrawer component accepts two childs. The first is the content layout, the second is the bottom drawer");

			ForceLayoutDrawing ();
		}

		public void Open ()
		{
			if (IsOpen)
				return;

			IsOpen = true;
			_shadowLayout.Clickable = true;

			if (_slideUpAniamtion == null) {
				var a = ObjectAnimator.OfFloat (_drawerLayout, TRANSLATION_ANIMATION, 0);
				a.AnimationEnd += delegate {
					if (OnDrawerOpen != null)
						OnDrawerOpen.Invoke ();
				};
				a.SetDuration (_animationDuration);
				_slideUpAniamtion = a;
			}
			_slideUpAniamtion.SetFloatValues (_drawerLayout.GetY (), _drawerOpenY);
			_slideUpAniamtion.Start ();

			if (_shadowEnabled) {
				if (_fadeInAniamtion == null) {
					var a = ObjectAnimator.OfFloat (_shadowLayout, ALPHA_ANIMATION, DrawerCloseShadowAlpha, _drawerOpenShadowAlpha);
					a.SetDuration (_animationDuration);
					_fadeInAniamtion = a;
				}
				_fadeInAniamtion.Start ();
			}
		}

		public void Close ()
		{
			if (!IsOpen)
				return;

			IsOpen = false;
			_shadowLayout.Clickable = false;

			if (_slideDownAniamtion == null) {
				var a = ObjectAnimator.OfFloat (_drawerLayout, TRANSLATION_ANIMATION, 0);
				a.AnimationEnd += delegate {
					if (OnDrawerClose != null)
						OnDrawerClose.Invoke ();
				};
				a.SetDuration (_animationDuration);
				_slideDownAniamtion = a;
			}

			_slideDownAniamtion.SetFloatValues (_drawerLayout.GetY (), _drawerClosedY);
			_slideDownAniamtion.Start ();

			if (_shadowEnabled) {
				if (_faceOutAniamtion == null) {
					var a = ObjectAnimator.OfFloat (_shadowLayout, ALPHA_ANIMATION, _drawerOpenShadowAlpha, DrawerCloseShadowAlpha);
					a.SetDuration (_animationDuration);
					_faceOutAniamtion = a;
				}
				_faceOutAniamtion.Start ();
			}
		}

		public void Toggle ()
		{
			if (IsOpen)
				Close ();
			else
				Open ();
		}

		public void ForceLayoutDrawing (bool force = false)
		{
			if (force) {
				_doIt = true;
				return;
			}

			var mon = new ContentViewHeightCalculator (this);
			mon.OnViewTreeObserverFinished += HandleOnViewTreeObserverFinished; 
			if (ViewTreeObserver.IsAlive)
				ViewTreeObserver.AddOnGlobalLayoutListener (mon);
				
			if (_shadowEnabled)
				_shadowLayout.Alpha = 0;

			IsOpen = false;
		}

		void HandleOnViewTreeObserverFinished (int parentHeight)
		{
			if (_originalDrawerHeight == 0) {
				if (DrawerHeightPercentage == 0) {
					_originalDrawerHeight = _drawerLayout.Height;
				}
				else _originalDrawerHeight = parentHeight * DrawerHeightPercentage / 100;
			}

			_drawerClosedY = parentHeight - DrawerCloseStartingY;
			_drawerOpenY = parentHeight - _originalDrawerHeight;
			if (_drawerOpenY < 0)
				_drawerOpenY = 0;

			var p = (FrameLayout.LayoutParams)_drawerLayout.LayoutParameters;
			p.Height = Height - _drawerOpenY;
			_drawerLayout.LayoutParameters = p;

			_drawerLayout.SetY (_drawerClosedY);
			_doIt = false;
		}

		View GetShadowLayout ()
		{
			var result = new View (Context);
			result.Click += delegate { Close (); };
			result.Clickable = false;
			var para = new ViewGroup.LayoutParams (ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
			result.SetBackgroundColor (Color.Black);
			result.LayoutParameters = para;
			result.Alpha = 0;
			return result;
		}

		protected override void Dispose (bool disposing)
		{
			OnDrawerOpen = null;
			OnDrawerClose = null;
			_contentLayout.DisposeIfNotNull ();
			_contentLayout = null;
			_drawerLayout.DisposeIfNotNull ();
			_drawerLayout = null;
			_shadowLayout.DisposeIfNotNull ();
			_shadowLayout = null;
			_slideUpAniamtion.DisposeIfNotNull ();
			_slideUpAniamtion = null;
			_slideDownAniamtion.DisposeIfNotNull ();
			_slideDownAniamtion = null;
			_fadeInAniamtion.DisposeIfNotNull ();
			_fadeInAniamtion = null;
			_faceOutAniamtion.DisposeIfNotNull ();
			_faceOutAniamtion = null;
			base.Dispose (disposing);
		}
	}

	class ContentViewHeightCalculator : Java.Lang.Object, ViewTreeObserver.IOnGlobalLayoutListener
	{
		View _view;

		public event Action<int> OnViewTreeObserverFinished;

		public ContentViewHeightCalculator (View view)
		{
			_view = view;
		}

		public void OnGlobalLayout ()
		{
			_view.ViewTreeObserver.RemoveGlobalOnLayoutListener (this);
			OnViewTreeObserverFinished.Invoke (_view.Height);
			_view = null;
			OnViewTreeObserverFinished = null;
		}
	}
}

