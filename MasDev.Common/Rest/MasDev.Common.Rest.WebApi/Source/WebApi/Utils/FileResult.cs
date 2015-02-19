using System;
using System.Web.Http;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.IO;
using System.Web;
using System.Net.Http.Headers;
using MasDev.Common;
using System.Collections.Generic;
using System.Linq;
using MasDev.Utils;
using System.Text;
using System.Diagnostics;


namespace MasDev.Rest.WebApi
{
	public class FileResult : IHttpActionResult
	{
		public static readonly FileCache Cache = new FileCache ();

		readonly string _filePath;
		readonly string _contentType;
		readonly HttpStatusCode _statusCode;
		readonly DateTime? _ifModifiedSince;
		readonly bool _useCache;

		public FileResult (IfModifiedSinceHeader ifModifiedSince, string filePath, string contentType = null)
		{
			if (filePath == null)
				throw new ArgumentNullException ("filePath");

			_filePath = filePath;
			_contentType = contentType;
			_statusCode = HttpStatusCode.OK;
			_ifModifiedSince = ifModifiedSince == null ? null : (DateTime?)ifModifiedSince.TimeUtc;
		}

		public FileResult (IfModifiedSinceHeader ifModifiedSince, string filePath, bool useCache, string contentType = null) : this (ifModifiedSince, filePath, contentType)
		{
			_useCache = useCache;
		}

		public FileResult (IfModifiedSinceHeader ifModifiedSince, string filePath, HttpStatusCode statusCode, string contentType = null) : this (ifModifiedSince, filePath, contentType)
		{
			_statusCode = statusCode;
		}

		public FileResult (IfModifiedSinceHeader ifModifiedSince, string filePath, HttpStatusCode statusCode, bool useCache, string contentType = null) : this (ifModifiedSince, filePath, statusCode, contentType)
		{
			_useCache = useCache;
		}

		public async Task<HttpResponseMessage> ExecuteAsync (CancellationToken cancellationToken)
		{
			FileCache.CachedFile cachedFile = null;
			FileInfo fileInfo = null;
			DateTime lastModifiedUtc;

			if (_useCache && Cache.Has (_filePath)) {
				cachedFile = Cache [_filePath];
				lastModifiedUtc = cachedFile.LastModifiedUtc;
			} else {
				fileInfo = new FileInfo (_filePath);
				lastModifiedUtc = fileInfo.LastWriteTimeUtc;
			}
				
			if (_ifModifiedSince.HasValue && _statusCode == HttpStatusCode.OK) {
				if (lastModifiedUtc <= _ifModifiedSince.Value)
					return new HttpResponseMessage (HttpStatusCode.NotModified);
			}
			
			var response = new HttpResponseMessage (HttpStatusCode.OK);
			if (!_useCache) {
				response.Content = new StreamContent (File.OpenRead (_filePath));
			} else {
				string stringContent;

				if (cachedFile == null) {
					using (var fileStream = new FileStream (_filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true)) {
						var sb = new StringBuilder ();
						var buffer = new byte[0x1000];
						int numRead;
						while ((numRead = await fileStream.ReadAsync (buffer, 0, buffer.Length)) != 0) {
							cancellationToken.ThrowIfCancellationRequested ();
							var text = Encoding.Unicode.GetString (buffer, 0, numRead);
							sb.Append (text);
						}
						stringContent = sb.ToString ();
					}
					Cache [_filePath] = new FileCache.CachedFile {
						Content = stringContent,
						LastModifiedUtc = fileInfo.LastWriteTimeUtc,
						Length = fileInfo.Length
					};
				} else {
					stringContent = cachedFile.Content;
				}

				response.Content = new StringContent (stringContent, Encoding.Unicode);
			}
					
			response.Content.Headers.LastModified = lastModifiedUtc;
			var contentType = _contentType ?? MimeMapping.GetMimeMapping (Path.GetExtension (_filePath));
			response.Content.Headers.ContentType = new MediaTypeHeaderValue (contentType);

			return response;
		}


		public static Task<HttpResponseMessage> CreateAsync (IfModifiedSinceHeader ifModifiedSince, FileInfo fi)
		{
			var result = new FileResult (ifModifiedSince, fi.FullName);
			return result.ExecuteAsync (CancellationToken.None);
		}
	}


	public class FileCache
	{
		public class CachedFile
		{
			public DateTime LastModifiedUtc { get; set; }

			public string Content { get; set; }

			public long Length { get; set; }
		}

		long _currentSize;
		Queue<string> _lru;
		readonly Dictionary<string, CachedFile> _cache;
		readonly object _lock;

		/// <summary>
		/// Gets or sets the maximum size of the cache (in bytes)
		/// </summary>
		/// <value>The max size.</value>
		public long MaxSize { get; set; }

		/// <summary>
		/// Gets the current size in bytes of the cache.
		/// </summary>
		/// <value>The current size</value>
		public double CurrentSize { get { return _currentSize; } }

		public FileCache ()
		{
			_cache = new Dictionary<string, CachedFile> ();
			_lru = new Queue<string> ();
			_currentSize = 0;
			_lock = new object ();
			MaxSize = long.MaxValue;
		}

		public FileCache (long maxSize) : this ()
		{
			MaxSize = maxSize;
		}

		public bool Has (string path)
		{
			return this [path] != null;
		}


		public CachedFile this [string path] {
			get { 
				lock (_lock) {
					return _cache.ContainsKey (path) ? _cache [path] : null;
				}
			}

			set { 
				lock (_lock) {
					Assert.NotNull (value);

					while (MaxSize < (_currentSize + value.Length)) {
						if (!_lru.Any ())
							throw new ArgumentException ("File too big for this cache. Maximum allowed: " + MaxSize + ", but got " + value.Length);


						var lruPath = _lru.Dequeue ();
						var lruFile = _cache [lruPath];
						_cache.Remove (lruPath);
						_currentSize -= lruFile.Length;
					}

					_currentSize += value.Length;
					_cache [path] = value; 
					_lru.Enqueue (path);
				}
			}
		}

		public void Invalidate ()
		{
			lock (_lock) {
				_cache.Clear ();
			}
		}

		public void Invalidate (string path)
		{
			lock (_lock) {			
				if (!_cache.ContainsKey (path))
					return;

				var file = _cache [path];
				_currentSize -= file.Length;

				var newLru = new Queue<string> ();
				while (_lru.Any ()) {
					var queueHead = _lru.Dequeue ();
					if (queueHead == path)
						continue;
					newLru.Enqueue (path);
				}
				_lru = newLru;
			}
		}
	}
}

