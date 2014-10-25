using System;
using Android.Content;
using Android.Views;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;
using Android.Support.V4.App;
using Android.Graphics;
using MasDev.Common.Droid.Utils;
using Android.Graphics.Drawables;

namespace MasDev.Common.Droid
{
	public static class DialogUtils
	{
		public sealed class LayoutDialog : DialogFragment
		{
			public Action<DialogFragment> OnDismissEx { get; private set; }
			public event Action<View> OnRootViewReady = delegate {};
			public event Action OnDismissDialog = delegate {};
			const string LAYOUT_ID_KEY = "1";

			public static LayoutDialog NewInstance (int layoutId, Action<DialogFragment> onClick)
			{
				var frag = new LayoutDialog ();
				frag.OnDismissEx = onClick;
				var bundle = new Bundle ();
				bundle.PutInt (LAYOUT_ID_KEY, layoutId);
				frag.Arguments = bundle;
				return frag;
			}
				
			public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
			{
				var view = inflater.Inflate(Arguments.GetInt (LAYOUT_ID_KEY), container, false);
				view.Click += delegate { OnDismissEx.Invoke (this); };
				return view;
			}

			public override void OnViewCreated (View view, Bundle savedInstanceState)
			{
				base.OnViewCreated (view, savedInstanceState);
				OnRootViewReady.Invoke (view);
			}

			public override Android.App.Dialog OnCreateDialog (Bundle savedInstanceState)
			{
				var dialog = base.OnCreateDialog (savedInstanceState);
				dialog.RequestWindowFeature ((int)WindowFeatures.NoTitle);
				dialog.Window.SetBackgroundDrawable (new ColorDrawable(Color.Transparent));
				return dialog;
			}

			public override void OnCreate (Bundle savedInstanceState)
			{
				base.OnCreate (savedInstanceState);
				SetStyle ((int)Android.App.DialogFragmentStyle.NoTitle, Android.Resource.Style.Theme);
			}
				
			public override void OnStop ()
			{
				base.OnStop ();
				if (OnDismissDialog != null)
					OnDismissDialog.Invoke ();
			}

			public new void Dispose()
			{
				OnDismissEx = null;
			}
		}

		public static void ShowDialog(Context ctx, string message, string title, bool isModal, string positiveButtonTextRes, string negativeButtonTextRes, Action onPositiveButtonListener, Action onNegativeButtonListener)
		{
			var builder = new Android.App.AlertDialog.Builder (ctx);
			if (message != null) builder.SetMessage (message);
			if (title != null) builder.SetTitle (title);
			if (positiveButtonTextRes != null) builder.SetPositiveButton (positiveButtonTextRes, delegate { onPositiveButtonListener.Invoke (); });
			if (negativeButtonTextRes != null) builder.SetNegativeButton (negativeButtonTextRes, delegate { onNegativeButtonListener.Invoke (); });

			var alert = builder.Create ();
			alert.SetCancelable (isModal);
			alert.Show ();
		}

		public static void ShowDialog(Context ctx, int? messageResourceId, int? titleRourceId, bool isModal, int? positiveButtonTextResId, int? negativeButtonTextResId, Action onPositiveButtonListener, Action onNegativeButtonListener)
		{
			var message = messageResourceId == null ? null : ctx.GetString (messageResourceId.Value);
			var title = titleRourceId == null ? null : ctx.GetString (titleRourceId.Value);
			var pos = positiveButtonTextResId == null ? null : ctx.GetString (positiveButtonTextResId.Value);
			var neg = negativeButtonTextResId == null ? null : ctx.GetString (negativeButtonTextResId.Value);


			ShowDialog (ctx, message, title, isModal, pos, neg, onPositiveButtonListener, onNegativeButtonListener);
		}

		public static Android.App.ProgressDialog ShowProgressDialog(Context context, string message, bool cancellable = true)
		{	
			var pd = new Android.App.ProgressDialog (context);
			pd.SetMessage (message);
			pd.SetCancelable (cancellable);
			pd.Show ();
			return pd;
		}

		public static Android.App.ProgressDialog ShowProgressDialog(Context context, int stringResId, bool cancellable = true)
		{	
			return ShowProgressDialog (context, context.GetString (stringResId), cancellable);
		}

		public static void ShowToast(Context ctx, string what, ToastLength lenght)
		{
			Toast.MakeText (ctx, what, lenght).Show ();
		}

		public static void ShowToast(Context ctx, int stringId, ToastLength lenght)
		{
			Toast.MakeText (ctx, stringId, lenght).Show ();
		}

		public static PopupMenu ShowPopupMenu(Context ctx, View anchor, ICollection<string> content, Action<string> onItemClick)
		{
			if (content == null || anchor == null)
				throw new ArgumentException ("Content and anchor must be not null");

			var popUpMenu = new PopupMenu (ctx, anchor);
			foreach(var item in content)
				popUpMenu.Menu.Add(item);

			popUpMenu.MenuItemClick += (snd, evt) => {
				string title = evt.Item.TitleFormatted.ToString ();
				onItemClick.Invoke (title);
			};

			popUpMenu.Show ();
			return popUpMenu;
		}

		public static PopupMenu ShowPopupMenu(Context ctx, View anchor, ICollection<string> content, Action<int> onItemClick)
		{
			if (content == null || anchor == null)
				throw new ArgumentException ("Content and anchor must be not null");

			var popUpMenu = new PopupMenu (ctx, anchor);
			int i = 0;
			const int noSense = 0;
			foreach (var item in content)
				popUpMenu.Menu.Add (noSense, i++, noSense, item);

			popUpMenu.MenuItemClick += (snd, evt) => onItemClick.Invoke (evt.Item.ItemId);

			popUpMenu.Show ();
			return popUpMenu;
		}

		public sealed class ContextualMenu : Java.Lang.Object, IDialogInterfaceOnClickListener 
		{
			readonly Action<int> _onItemClick;
			readonly Android.App.AlertDialog _alertDialog;

			public ContextualMenu(Context ctx, string title, List<string> items, bool isModal, Action<int> onItemClick)
			{
				_onItemClick = onItemClick;

				var builder = new Android.App.AlertDialog.Builder (ctx);
				builder.SetAdapter (new ArrayAdapter<string>(ctx, Android.Resource.Layout.SimpleListItem1, items), this);

				_alertDialog = builder.Create ();
				_alertDialog.SetCancelable (!isModal);
				_alertDialog.SetTitle (title);
			}

			public void OnClick (IDialogInterface dialog, int which)
			{
				_onItemClick.Invoke (which);
			}

			public void Show()
			{
				_alertDialog.Show ();
			}
		}
			
		public static void ShowAcceptLicenceTerms(Context ctx, string linkToTerms, string linkText, string message, string title, string positiveButtonTextRes, string negativeButtonTextRes, Action onPositiveButtonListener, Action onNegativeButtonListener)
		{
			var builder = new Android.App.AlertDialog.Builder (ctx);
			if (title != null) builder.SetTitle (title);
			var ll = new LinearLayout (ctx);
			var lp = new ViewGroup.LayoutParams (ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);
			ll.LayoutParameters = lp;
			ll.Orientation = Orientation.Vertical;
			var padding = ApplicationUtils.ConvertDpToPixel (ctx, 5);
			ll.SetPadding (padding, padding, padding, padding);
			var textView = new TextView (ctx);
			textView.Text = message;
			textView.Gravity = GravityFlags.Center;
			ll.AddView (textView);

			textView = new TextView (ctx);
			textView.Text = linkText;
			textView.SetPadding (0, ApplicationUtils.ConvertDpToPixel (ctx, 10), 0, 0);
			textView.PaintFlags = textView.PaintFlags | PaintFlags.UnderlineText;
			textView.SetTextColor (Color.Blue);
			textView.Gravity = GravityFlags.Center;
			textView.Click += (sender, e) => ApplicationUtils.Intents.StartBrowserActivity (ctx, linkToTerms, ApplicationUtils.Intents.VoidDelegate);
			ll.AddView (textView);

			builder.SetView (ll);
			if (positiveButtonTextRes != null) builder.SetPositiveButton (positiveButtonTextRes, delegate { onPositiveButtonListener.Invoke (); });
			if (negativeButtonTextRes != null) builder.SetNegativeButton (negativeButtonTextRes, delegate { onNegativeButtonListener.Invoke (); });

			var alert = builder.Create ();
			alert.SetCancelable (false);
			alert.Show ();
		}
	}
}

