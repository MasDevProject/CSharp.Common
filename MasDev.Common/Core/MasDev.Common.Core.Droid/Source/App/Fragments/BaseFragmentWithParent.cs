using MasDev.Droid.Utils;

namespace MasDev.Droid.App
{
	public abstract class BaseFragmentWithParent<TParent> : BaseFragment where TParent : class
	{
		protected TParent Parent { get; set; }

		public override void OnCreate (Android.OS.Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);
			if (Parent == null)
				Parent = FragmentUtils.EnsureImplements<TParent> (this);
		}
	}
}

