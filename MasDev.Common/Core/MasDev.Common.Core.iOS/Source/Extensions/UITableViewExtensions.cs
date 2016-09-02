using UIKit;
using Foundation;

namespace MasDev.Common
{
	public static class UITableViewExtensions
	{
		public static void ScrollToRow (this UITableView tableView, int row, int section, bool animated)
		{
			tableView.ScrollToRow (NSIndexPath.FromRowSection (row, section), UITableViewScrollPosition.Top, animated);
		}
	}
}