using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace MasDev.Common
{
	/// <summary>
	/// Base on SharpZipLib.Portble Library
	/// </summary>
	public class Zipper
	{
		/// <summary>
		/// Unzip the specified zipFilePath, destinationPath and ingoreHiddenFilesAndDirectories.
		/// </summary>
		/// <param name="zipFilePath">Zip file path.</param>
		/// <param name="destinationPath">Destination path. If null files will be extracted in the same folder of zipFile</param>
		/// <param name="ingoreHiddenFilesAndDirectories">If set to <c>true</c> ingore hidden files and directories.</param>
		public void Unzip (string zipFilePath, string destinationPath = null, bool ingoreHiddenFilesAndDirectories = true)
		{
			using (var s = new ZipInputStream (File.OpenRead (zipFilePath))) 
			{
				ZipEntry theEntry;
				while ((theEntry = s.GetNextEntry ()) != null) 
				{
					var directoryName = Path.GetDirectoryName (theEntry.Name);
					var fileName = Path.GetFileName (theEntry.Name);

					if (ingoreHiddenFilesAndDirectories && (directoryName.StartsWith (".") || directoryName.StartsWith ("_")))
						continue;

					if (directoryName.Length > 0)
						Directory.CreateDirectory (directoryName);

					if (fileName != String.Empty) 
					{
						var unzippedFileDestination = Path.Combine (
							destinationPath ?? zipFilePath.Replace (Path.GetFileName (zipFilePath), string.Empty), theEntry.Name
						);
						using (var streamWriter = File.Create (unzippedFileDestination)) 
						{
							int size;
							var data = new byte [2048];
							while (true) {
								size = s.Read (data, 0, data.Length);
								if (size > 0)
									streamWriter.Write (data, 0, size);
								else
									break;
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Zip the specified directoryPath, outputPath and compressionLevel.
		/// </summary>
		/// <param name="zipFilePath">Directory path.</param>
		/// <param name="outputPath">Output path.</param>
		/// <param name="compressionLevel">Compression level: from 0 (store only) to 9 (means best compression)</param>
		public void Zip (string zipFilePath, string outputPath, int compressionLevel = 6)
		{
			try
			{
				// Depending on the directory this could be very large and would require more attention
				// in a commercial package.
				var filenames = Directory.GetFiles (zipFilePath);

				// 'using' statements guarantee the stream is closed properly which is a big source
				// of problems otherwise.  Its exception safe as well which is great.
				using (var s = new ZipOutputStream(File.Create(outputPath))) 
				{
					s.SetLevel(compressionLevel);

					var buffer = new byte [4096];

					foreach (var file in filenames) 
					{
						var entry = new ZipEntry (Path.GetFileName (file));
						entry.DateTime = DateTime.Now;
						s.PutNextEntry (entry);

						using (var fs = File.OpenRead (file))
						{
							// Using a fixed size buffer here makes no noticeable difference for output but keeps a lid on memory usage.
							int sourceBytes;
							do {
								sourceBytes = fs.Read (buffer, 0, buffer.Length);
								s.Write (buffer, 0, sourceBytes);
							} while (sourceBytes > 0);
						}
					}

					s.Finish ();
					s.Close ();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine ("Exception during processing {0}", ex);
			}
		}
	}
}

