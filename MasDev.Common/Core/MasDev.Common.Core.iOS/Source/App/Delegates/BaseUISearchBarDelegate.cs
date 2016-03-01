using System;
using System.Threading;
using UIKit;
using System.Threading.Tasks;

namespace MasDev.Common
{
	public class BaseUISearchBarDelegate : UISearchBarDelegate
	{
		public event Action<string, CancellationToken> OnSeachRequest;

		protected virtual int SearchTaskDelay { get; } = 400;

		CancellationTokenSource cancellationTokenSource;

		public override async void TextChanged (UISearchBar searchBar, string searchText)
		{
			if (cancellationTokenSource != null)
				cancellationTokenSource.Cancel ();
			cancellationTokenSource = new CancellationTokenSource ();

			await DispatchChanges (cancellationTokenSource.Token, searchText);
		}

		protected virtual async Task DispatchChanges(CancellationToken token, String searchText = null)
		{
			await Task.Delay (SearchTaskDelay);

			if (token.IsCancellationRequested)
				return;

			try
			{
				if(OnSeachRequest != null)
					OnSeachRequest.Invoke(searchText, token);
			}
			catch (OperationCanceledException)
			{
				// Task cancelled
			}
		}
	}
}