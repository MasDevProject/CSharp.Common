using Android.Support.V7.App;

namespace MasDev.Common.Droid.App
{
	public enum ActivityStates {
		Running,
		Destroyed,
		Pause,
	}

	public class FragmentActivityWithStates : ActionBarActivity
	{
		public ActivityStates State { get; private set; }

		protected override void OnResume ()
		{
			State = ActivityStates.Running;
			base.OnResume ();
		}

		protected override void OnPause ()
		{
			State = ActivityStates.Pause;
			base.OnPause ();
		}

		protected override void OnDestroy ()
		{
			State = ActivityStates.Destroyed;
			base.OnDestroy ();
		}
	}
}

