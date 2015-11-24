using Android.Support.V7.Widget;
using Android.Views;
using System;

namespace MasDev.Droid.Utils
{
	public class ViewHolderWithPositionListener : RecyclerView.ViewHolder, View.IOnClickListener
	{
		public event Action<int> OnItemClick;

		public ViewHolderWithPositionListener (View rootView) : base (rootView)
		{
			rootView.SetOnClickListener (this);
		}

		public ViewHolderWithPositionListener (View rootView, Action<int> onItemClick) : this (rootView)
		{
			OnItemClick += onItemClick;
		}

		public void OnClick (View v)
		{
			if (OnItemClick != null)
				OnItemClick (LayoutPosition);
		}
	}
}

