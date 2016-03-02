using Foundation;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace MasDev.Common
{
	public abstract class BaseDownloadService : IDownloadDelegateConfigurator
	{
		NSUrlSession _session;
		NSUrlSessionDelegate _sessionDelegate;

		protected NSUrlSession Session
		{
			get
			{
				if (_session == null)
					_session = InitializeSession ();

				return _session;
			}
		}

		protected virtual NSUrlSession InitializeSession()
		{
			NSUrlSession session = null;

			using (var configuration = NSUrlSessionConfiguration.CreateBackgroundSessionConfiguration (SessionIdentifier))
			{
				ConfigureSession (configuration);

				_sessionDelegate = CreateSessionDelegate();

				session = NSUrlSession.FromConfiguration (configuration, _sessionDelegate, null);
			}

			return session;
		}

		// Abstract members

		protected abstract string SessionIdentifier { get; }

		protected abstract void ConfigureSession(NSUrlSessionConfiguration configuration);

		public abstract Action GetBackgroundSessionCompletionHandler ();

		public abstract void HandleDownloadCompleted (string url, string destinationPath);

		public abstract void HandleDownloadsFinished ();

		protected virtual NSUrlSessionDelegate CreateSessionDelegate()
		{
			return new BaseSessionDownloadDelegate (this, HandleDownloadCompleted);
		}

		// Public methods

		public void ResetSession()
		{
			if (_session == null)
				return;

			_sessionDelegate = null;

			_session.InvalidateAndCancel();
			_session = null;
		}

		public async Task<IEnumerable<NSUrlSessionTask>> GetPendingTasks()
		{
			var tasks = await Session.GetTasks2Async ();

			if (tasks != null && tasks.DownloadTasks != null)
				return (tasks.DownloadTasks).ToList ();

			return null;
		}

		public void EnqueueTask(string url, string destinationPath)
		{
			var task = CreateTask (url);

			task.TaskDescription = destinationPath;
			task.Resume ();
		}

		// Utils

		protected virtual NSUrlSessionDownloadTask CreateTask(string url)
		{
			var downloadURL = NSUrl.FromString (new NSString(url));
			var request = NSUrlRequest.FromUrl (downloadURL);

			return Session.CreateDownloadTask (request);
		}
	}
}