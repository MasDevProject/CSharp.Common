using MasDev.Common;
using MasDev.Patterns.Injection;
using Android.Views;
using Android.OS;

namespace MasDev.Droid.App
{
	public class ViewModelFragment<TParent, TViewModel> : BaseFragmentWithParent<TParent> where TParent : class where TViewModel : class, IViewModel
	{
		protected TViewModel ViewModel;

		public ViewModelFragment ()
		{
			ViewModel = Injector.Resolve<TViewModel> ();
		}

		public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			ViewModel.SubscribeEvents ();
			return base.OnCreateView (inflater, container, savedInstanceState);
		}

		public override void OnDestroyView ()
		{
			ViewModel.UnsubscribeEvents ();
			base.OnDestroyView ();
		}
	}
}

