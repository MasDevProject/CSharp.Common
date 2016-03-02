using System;
using Foundation;
using System.IO;

namespace MasDev.Common
{
	public interface IDownloadDelegateConfigurator
	{
		Action GetBackgroundSessionCompletionHandler();

		void HandleDownloadCompleted (string url, string destinationPath);

		void HandleDownloadProgress (string url, float progress);

		void HandleDownloadsFinished();
	}

	public class BaseSessionDownloadDelegate : NSUrlSessionDownloadDelegate
	{
		readonly IDownloadDelegateConfigurator _configurator;

		public BaseSessionDownloadDelegate(IDownloadDelegateConfigurator configurator)
		{
			_configurator = configurator;
		}

		public override void DidFinishDownloading (NSUrlSession session, NSUrlSessionDownloadTask downloadTask, NSUrl location)
		{
			#if DEBUG

			Console.WriteLine ("File downloaded in : {0}", location);

			#endif

			var fileManager = NSFileManager.DefaultManager;
			var downloadUrl = downloadTask.Response.Url.ToString ();
			var destinationPath = downloadTask.TaskDescription;

			NSError errorCopy, errorRemove;
			var fileName = Path.GetFileName (destinationPath);
			var fileFolder = destinationPath.Replace(fileName, string.Empty);

			fileManager.Remove (destinationPath, out errorRemove);

			Directory.CreateDirectory(fileFolder);

			fileManager.Copy (location.Path, destinationPath, out errorCopy);

			#if DEBUG

			if(errorCopy != null)
				Console.WriteLine (errorCopy.LocalizedDescription);

			#endif

			_configurator.HandleDownloadCompleted (downloadUrl, destinationPath);
		}

		public override void DidFinishEventsForBackgroundSession (NSUrlSession session)
		{
			var handler = _configurator.GetBackgroundSessionCompletionHandler ();

			// call completion handler when you're done
			if (handler != null) 
			{
				_configurator.HandleDownloadsFinished ();

				handler.Invoke ();
				handler = null;
			}
		}

		public override void DidWriteData (NSUrlSession session, NSUrlSessionDownloadTask downloadTask, long bytesWritten, long totalBytesWritten, long totalBytesExpectedToWrite)
		{
			#if DEBUG
				Console.WriteLine("["+ downloadTask.TaskIdentifier +"] download: " + Math.Round(((float) totalBytesWritten / totalBytesExpectedToWrite) * 100, 2) + "%");
			#endif

			_configurator.HandleDownloadProgress (downloadTask.Response.Url.ToString(), (float) Math.Round (((float)totalBytesWritten / totalBytesExpectedToWrite) * 100, 2));
		}
	}
}