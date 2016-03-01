using System;
using Foundation;
using UIKit;
using MasDev.iOS.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace MasDev.Common
{
	public abstract class BaseNSUrlSessionManager
	{
		public abstract string SessionIdentifier { get; }

		protected NSUrlSession Session;
		protected NSUrlSessionDelegate SessionDelegate;

		protected NSUrlSession CurrentSession
		{
			get
			{
				if (Session == null)
					InitializeSession ();

				return Session;
			}
		}

		protected virtual void InitializeSession()
		{
			using (var configuration = UIDevice.CurrentDevice.CheckSystemVersion(8, 0) ? 
				NSUrlSessionConfiguration.CreateBackgroundSessionConfiguration (SessionIdentifier) :
				NSUrlSessionConfiguration.BackgroundSessionConfiguration (SessionIdentifier))
			{
				CustomizeConfiguration(configuration);

				SessionDelegate = InitializeSessionDelegate();

				Session = NSUrlSession.FromConfiguration (configuration, SessionDelegate, null);
			}
		}

		// Abstract methods

		protected abstract void CustomizeConfiguration(NSUrlSessionConfiguration configuration);

		protected abstract NSUrlSessionDelegate InitializeSessionDelegate();

		public void ResetSession()
		{
			if (CurrentSession == null)
				return;

			SessionDelegate = null;

			CurrentSession.InvalidateAndCancel();
			Session = null;
		}

		public virtual NSUrlSessionDownloadTask CreateTask(string url)
		{
			var downloadURL = NSUrl.FromString (url.ToNSString());
			var request = NSUrlRequest.FromUrl (downloadURL);

			return Session.CreateDownloadTask (request);
		}

		public virtual async Task<IEnumerable<NSUrlSessionTask>> GetPendingTasks()
		{
			var tasks = await Session.GetTasks2Async ();

			if (tasks != null && tasks.DownloadTasks != null)
				return (tasks.DownloadTasks).ToList ();

			return null;
		}

		public virtual void EnqueueTask(NSUrlSessionDownloadTask task)
		{
			task.Resume ();
		}
	}
}