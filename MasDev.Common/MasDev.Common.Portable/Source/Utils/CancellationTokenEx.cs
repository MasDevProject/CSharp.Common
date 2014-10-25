using System.Threading;

namespace MasDev.Common
{
	public class CancellationTokenEx
	{
		CancellationTokenSource _source;
		public CancellationToken CancellationToken { get { return _source.Token; }}

		public CancellationTokenEx ()
		{
			_source = new CancellationTokenSource ();
		}

		public void RequestCancellation()
		{
			_source.Cancel ();
		}

		public void ThrowIfCancellationRequested()
		{
			CancellationToken.ThrowIfCancellationRequested ();
		}

		public bool IsCancellationRequested {
			get {
				return CancellationToken.IsCancellationRequested;
			}
		}

		public void Reset()
		{
			_source = new CancellationTokenSource ();
		}
	}
}

